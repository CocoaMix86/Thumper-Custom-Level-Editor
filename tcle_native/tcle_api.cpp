#include <glad/gl.h>
#include <GLFW/glfw3.h>

#include <cstdlib>
#include <vector>
#include <string>
#include <filesystem>
#include <fstream>

#include <Windows.h>

#define TCLE_ERR_GEN 0
#define TCLE_OK 1
#define TCLE_ERR_WIN 2

struct TCLEGlobalData final {
    void* pixelBuffer = nullptr;
    GLFWwindow* window = nullptr;
};

namespace {
    TCLEGlobalData gData;

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
}

extern "C" __declspec(dllexport) void* __cdecl tcle_native_draw(int width, int height) {
    // Safety
    if (width < 1 || height < 1) return nullptr;



    GLuint fbo;
    glGenFramebuffers(1, &fbo);
    glBindFramebuffer(GL_FRAMEBUFFER, fbo);

    GLuint colorAttachment;
    glGenTextures(1, &colorAttachment);
    glBindTexture(GL_TEXTURE_2D, colorAttachment);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, nullptr);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

    glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, colorAttachment, 0);

    GLuint vao;
    glGenVertexArrays(1, &vao);
    glBindVertexArray(vao);

    GLuint vbo;
    glGenBuffers(1, &vbo);

    float data[] = { -1.0f, 1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 1.0f, -1.0f };
    glBindBuffer(GL_ARRAY_BUFFER, vbo);
    glBufferData(GL_ARRAY_BUFFER, sizeof(data), data, GL_STATIC_DRAW);

    glEnableVertexAttribArray(0);
    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 8, 0);

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

    GLuint program = glCreateProgram();
    glAttachShader(program, vertShader);
    glAttachShader(program, fragShader);
    glLinkProgram(program);
    glDeleteShader(vertShader);
    glDeleteShader(fragShader);

    glGetProgramiv(program, GL_INFO_LOG_LENGTH, &infoLogLength);

    if (infoLogLength > 0) {
        std::vector<char> str;
        str.resize(infoLogLength);
        glGetProgramInfoLog(fragShader, infoLogLength, nullptr, str.data());
        MessageBoxA(NULL, "Shader Link Error", str.data(), 0);
    }



    glUseProgram(program);
    glUniform1f(glGetUniformLocation(program, "iTime"), static_cast<float>(glfwGetTime()));
    glUniform2f(glGetUniformLocation(program, "iResolution"), width, height);


    glViewport(0, 0, width, height);
    glClearColor(0.7f, 0.8f, 0.9f, 1.0f);
    glClear(GL_COLOR_BUFFER_BIT);

    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

    if (gData.pixelBuffer) free(gData.pixelBuffer);
    gData.pixelBuffer = malloc(width * height * 4);

    glFlush();
    glFinish();
    glReadPixels(0, 0, width, height, GL_BGRA, GL_UNSIGNED_BYTE, gData.pixelBuffer);

    glDeleteFramebuffers(1, &fbo);
    glDeleteTextures(1, &colorAttachment);
    glDeleteProgram(program);


    // Get pixels from renderer framebuffer color attachment

    return gData.pixelBuffer;
}

extern "C" __declspec(dllexport) int __cdecl tcle_native_init(void) {
    if (!glfwInit()) return TCLE_ERR_WIN;
    glfwWindowHint(GLFW_VISIBLE, GLFW_FALSE);
    gData.window = glfwCreateWindow(200, 200, "TCLE Native", nullptr, nullptr);
    if (!gData.window) {
        glfwTerminate();
        return TCLE_ERR_WIN;
    }

    glfwMakeContextCurrent(gData.window);

    gladLoadGL(&glfwGetProcAddress);


    return TCLE_OK;
}

extern "C" __declspec(dllexport) void __cdecl tcle_native_uninit(void) {
    if (gData.pixelBuffer) free(gData.pixelBuffer);
    if (gData.window) {
        glfwMakeContextCurrent(nullptr);
        glfwDestroyWindow(gData.window);
        glfwTerminate();
    }
}