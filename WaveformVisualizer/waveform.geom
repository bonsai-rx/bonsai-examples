#version 400
uniform int cols;
layout(lines) in;
layout(line_strip, max_vertices = 2) out;

void main()
{
  if (gl_PrimitiveIDIn % cols != cols - 1)
  {
    gl_Position = gl_in[0].gl_Position;
    EmitVertex();

    gl_Position = gl_in[1].gl_Position;
    EmitVertex();
    EndPrimitive();
  }
}
