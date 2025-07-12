#version 330 core

in vec2 fUV;

uniform sampler2D textureUnit;

out vec4 outColor;

void main() 
{
	vec4 color = texture(textureUnit, fUV);

	if (color.a == 0)
	{
		discard;
	}

	outColor = color;
}