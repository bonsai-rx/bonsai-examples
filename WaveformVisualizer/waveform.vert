#version 400
uniform int rows;
uniform int cols;
uniform vec2 scale = vec2(1, 1);
uniform vec2 shift;
in float amplitude;

void main()
{
  int rowID = gl_VertexID / cols;
  int sampleID = gl_VertexID % cols;
  float xoffset = 2.0 * sampleID / cols - 1;
  float yoffset = (2.0 * rowID + 1) / rows - 1;
  vec2 vp = vec2(xoffset, yoffset + amplitude);
  gl_Position = vec4(vp * scale + shift, 0.0, 1.0);
}
