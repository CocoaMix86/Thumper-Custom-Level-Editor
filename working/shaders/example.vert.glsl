#version 330 core

void main(void) {
    vec2 uv = vec2(gl_VertexID & 2, 1 - ((gl_VertexID << 1) & 2));
	gl_Position = vec4(uv * 2.0 - 1.0, 0.0, 1.0);
}