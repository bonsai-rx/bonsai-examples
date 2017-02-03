#version 400
uniform mat4 modelview;
uniform mat4 projection;
in vec3 vp;
in vec3 vn;
out vec3 position;
out vec3 normal;

void main()
{
  mat4 normalmat = transpose(inverse(modelview));
  vec4 v = modelview * vec4(vp, 1.0);
  gl_Position = projection * v;
  position = vec3(v);
  normal = normalize(vec3(normalmat * vec4(vn, 0.0)));
}
