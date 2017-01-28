#version 400
in vec2 texCoord;
out vec4 fragColor;
uniform vec4 color;

void main()
{
  // position of the fragment inside the unit circle
  vec2 position = 2 * texCoord - 1;

  // fragment is inside the circle when the length is smaller than one
  float scale = length(position) < 1 ? 1.0 : 0.0;
  fragColor = color * scale;
}
