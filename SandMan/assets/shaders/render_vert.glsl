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
uniform mat4 model;

uniform vec4 uv_coords;

uniform int centered;


out vec2 uv;

void main() {
    vec2 vertex = vertices[gl_VertexID];
    
    uv = vertex * uv_coords.zw + uv_coords.xy;
    
    vec2 pos = (model * vec4((vertex - 0.5f) * size, 0, 1)).xy + float(1 - centered) * (size * 0.5f) + position - camera;
    
    gl_Position = projection * vec4(pos, 0, 1);
}