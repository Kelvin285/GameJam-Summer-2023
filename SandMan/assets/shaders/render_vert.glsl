#version 430 core

vec2 vertices[] = {
    vec2(0, 0),
    vec2(0, 1),
    vec2(1, 1),
    vec2(1, 1),
    vec2(1, 0),
    vec2(0, 0)
};

out vec2 uv;

void main() {
    vec2 vertex = vertices[gl_VertexID];
    
    uv = vertex;
    
    gl_Position = vec4(vertex * 2 - 1, 0, 1);
}