#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aColor; // the color variable has attribute position 1
layout (location = 2) in vec2 aTexCoord;


out vec2 TexCoord;
out vec3 ourColor;

uniform mat4 transform;


void main()
{
   gl_Position = transform * vec4(aPos, 1.0);
	ourColor = aColor;
	TexCoord = aTexCoord;
};