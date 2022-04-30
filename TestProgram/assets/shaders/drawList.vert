#version 330 core

layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vTexCoords;
layout (location = 2) in vec4 vColor;
layout (location = 3) in uint vModelID;

uniform mat4 uModelPos;
uniform mat4 uView;
uniform mat4 uProjection;
uniform vec3 uViewPos;

out vec3 fNormal;
out vec3 fPos;
out vec2 fTexCoords;

void main()
{
	gl_Position = uProjection * uView * uModelPos * vec4(vPos, 1.0);
	fPos        = vec3(uModelPos * vec4(vPos, 1.0));
	fNormal     = mat3(transpose(inverse(uModelPos))) * vPos;
	fTexCoords  = vTexCoords;
}
