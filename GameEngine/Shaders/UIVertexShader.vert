#version 330 core

layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec2 vUV;

uniform mat4 projection;
uniform mat4 model;

out vec2 fUV;

void main()
{
	fUV = vUV;

	gl_Position = projection * model * vec4(vPosition, 1.0);
}