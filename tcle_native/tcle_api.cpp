#include <glad/gl.h>
#include <GLFW/glfw3.h>
#include <cstdlib>

#define TCLE_ERR_GEN 0
#define TCLE_OK 1
#define TCLE_ERR_WIN 2

struct TCLEGlobalData final {
    void* pixelBuffer = nullptr;
    GLFWwindow* window = nullptr;
};

namespace {
    TCLEGlobalData gData;
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
    float data[] = {-0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.5f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f};
    glBindBuffer(GL_ARRAY_BUFFER, vbo);
    glBufferData(GL_ARRAY_BUFFER, sizeof(data), data, GL_STATIC_DRAW);
    
    glEnableVertexAttribArray(0);
    glEnableVertexAttribArray(1);
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 24, 0);
    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 24, (void*)12);

    char const* vertSource = R"(
#version 330 core

layout(location = 0) in vec3 iPosition;
layout(location = 1) in vec3 iColor;

out vec3 vColor;

void main(void) {
    gl_Position = vec4(iPosition, 1.0);
    vColor = iColor;
}
)";

    char const* fragSource = R"(
#version 330 core

in vec3 vColor;

layout(location = 0) out vec4 oColor;

void main(void) {
    oColor = vec4(vColor, 1.0);
}
)";
    GLuint vertShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vertShader, 1, &vertSource, nullptr);

    GLuint fragShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fragShader, 1, &fragSource, nullptr);

    GLuint program = glCreateProgram();
    glAttachShader(program, vertShader);
    glAttachShader(program, fragShader);
    glLinkProgram(program);
    glDeleteShader(vertShader);
    glDeleteShader(fragShader);

    glUseProgram(program);

    glViewport(0, 0, width, height);
    glClearColor(0.7f, 0.8f, 0.9f, 1.0f);
    glClear(GL_COLOR_BUFFER_BIT);

    glDrawArrays(GL_TRIANGLES, 0, 3);

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