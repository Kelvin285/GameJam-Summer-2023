#version 430 core

vec2 vertices[] = {
    vec2(0, 0),
    vec2(0, 1),
    vec2(1, 1),
    vec2(1, 1),
    vec2(1, 0),
    vec2(0, 0)
};

uniform vec2 position;
uniform vec2 size;

uniform vec2 camera;

uniform vec2 screen_size;

uniform mat4 projection;


out vec2 uv;

void main() {
    vec2 vertex = vertices[gl_VertexID];
    
    uv = vertex;
    
    vec2 pos = vertex * size + position - camera;
    
    gl_Position = projection * vec4(pos, 0, 1);
}