#version 330 core

layout(location = 0) in vec2 iPosition;

void main(void) {
    gl_Position = vec4(iPosition, 0.0, 1.0);
}