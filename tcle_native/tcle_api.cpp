#include "tcle_window.hpp"

#include <cstdlib>
#include <vector>
#include <string>
#include <filesystem>
#include <fstream>
#include <memory>
#include <stdexcept>

#include <Windows.h>

namespace {
std::string read_file(std::filesystem::path const& path) {
    std::ifstream stream(path, std::ios::in | std::ios::binary);
    if (!stream) return "";
    std::string string;
    stream.seekg(0, std::ios::end);
    string.resize(stream.tellg());
    stream.seekg(0, std::ios::beg);
    stream.read(string.data(), string.size());
    return string;
}

class TCLENativeEngine final {
public:
    TCLENativeEngine() {
        mWindow = tcle::Window(nullptr);
        if (!mWindow.handle()) throw std::runtime_error("Failed to create window");
        glfwMakeContextCurrent(mWindow.handle());
        if (!gladLoadGL(&glfwGetProcAddress)) throw std::runtime_error("Failed to load graphics context");

        glGenVertexArrays(1, &mEmptyVao);
        glBindVertexArray(mEmptyVao); // Create object store
        glBindVertexArray(0);

        reload();
    }

    void resize_framebuffer(int width, int height) {
        if (mWidth == width && mHeight == height) return;

        mWidth = width;
        mHeight = height;
        mPixels = std::make_unique<char[]>(width * height * 4);

        if (mFbo) glDeleteFramebuffers(1, &mFbo);
        if (mColorAttachment) glDeleteTextures(1, &mColorAttachment);

        glGenFramebuffers(1, &mFbo);
        glBindFramebuffer(GL_FRAMEBUFFER, mFbo);
        glGenTextures(1, &mColorAttachment);
        glBindTexture(GL_TEXTURE_2D, mColorAttachment);
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, nullptr);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
        glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, mColorAttachment, 0);
    }

    void reload() {
        std::string vertSourceF = read_file("shaders/example.vert.glsl");
        char const* vertSource = vertSourceF.c_str();
        std::string fragSourceF = read_file("shaders/example.frag.glsl");
        char const* fragSource = fragSourceF.c_str();

        GLuint vertShader = glCreateShader(GL_VERTEX_SHADER);
        glShaderSource(vertShader, 1, &vertSource, nullptr);
        glCompileShader(vertShader);

        GLint infoLogLength;
        glGetShaderiv(vertShader, GL_INFO_LOG_LENGTH, &infoLogLength);

        if (infoLogLength > 0) {
            std::vector<char> str;
            str.resize(infoLogLength);
            glGetShaderInfoLog(vertShader, infoLogLength, nullptr, str.data());
            MessageBoxA(NULL, "Shader Compilation Error", str.data(), 0);
        }

        GLuint fragShader = glCreateShader(GL_FRAGMENT_SHADER);
        glShaderSource(fragShader, 1, &fragSource, nullptr);
        glCompileShader(fragShader);

        glGetShaderiv(fragShader, GL_INFO_LOG_LENGTH, &infoLogLength);

        if (infoLogLength > 0) {
            std::vector<char> str;
            str.resize(infoLogLength);
            glGetShaderInfoLog(fragShader, infoLogLength, nullptr, str.data());
            MessageBoxA(NULL, "Shader Compilation Error", str.data(), 0);
        }

        if (mProgram) glDeleteProgram(mProgram);

        mProgram = glCreateProgram();
        glAttachShader(mProgram, vertShader);
        glAttachShader(mProgram, fragShader);
        glLinkProgram(mProgram);
        glDeleteShader(vertShader);
        glDeleteShader(fragShader);

        glGetProgramiv(mProgram, GL_INFO_LOG_LENGTH, &infoLogLength);

        if (infoLogLength > 0) {
            std::vector<char> str;
            str.resize(infoLogLength);
            glGetProgramInfoLog(mProgram, infoLogLength, nullptr, str.data());
            MessageBoxA(NULL, "Shader Link Error", str.data(), 0);
        }
    }

    void draw(int width, int height) {
        resize_framebuffer(width, height);

        glBindFramebuffer(GL_FRAMEBUFFER, mFbo);
        glViewport(0, 0, width, height);

        glClearColor(0.7f, 0.8f, 0.9f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);

        glBindVertexArray(mEmptyVao);
        glUseProgram(mProgram);
        glUniform1f(glGetUniformLocation(mProgram, "iTime"), static_cast<float>(glfwGetTime()));
        glUniform2f(glGetUniformLocation(mProgram, "iResolution"), width, height);
        glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

        glFlush();
        glFinish();
        glReadPixels(0, 0, width, height, GL_BGRA, GL_UNSIGNED_BYTE, mPixels.get());
    }

    ~TCLENativeEngine() noexcept {
        if (mFbo) glDeleteFramebuffers(1, &mFbo);
        if (mColorAttachment) glDeleteTextures(1, &mColorAttachment);

        if (mProgram) glDeleteProgram(mProgram);
        if (mEmptyVao) glDeleteVertexArrays(1, &mEmptyVao);

        glfwMakeContextCurrent(nullptr);
    }

    tcle::Window mWindow;
    std::unique_ptr<char[]> mPixels;

    GLuint mEmptyVao = 0;

    GLuint mProgram = 0;

    // Framebuffer
    int mWidth = 0, mHeight = 0;
    GLuint mFbo = 0;
    GLuint mColorAttachment = 0;
};

std::string gLastError;
std::unique_ptr<TCLENativeEngine> gEngine;

} // namespace <unnamed>

extern "C" __declspec(dllexport) void* __cdecl tcle_native_draw(int width, int height) {
    // Safety
    if (width < 1 || height < 1) return nullptr;

    gEngine->draw(width, height);

    return gEngine->mPixels.get();
}

extern "C" __declspec(dllexport) void __cdecl tcle_native_reload(void) {
    gEngine->reload();
}

extern "C" __declspec(dllexport) int __cdecl tcle_native_init(void) {
    try {
        gEngine = std::make_unique<TCLENativeEngine>();
        return true;
    }
    catch (std::exception const& e) {
        gLastError = e.what();
        return false;
    }
}

extern "C" __declspec(dllexport) void __cdecl tcle_native_uninit(void) {
    gEngine.reset();
}