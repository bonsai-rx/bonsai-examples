#version 400
uniform vec2 scale = vec2(1, 1);
uniform vec2 shift;
in vec2 vp;

void main()
{
  gl_Position = vec4(vp * scale + shift, 0.0, 1.0);
}
