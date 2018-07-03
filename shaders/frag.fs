#define SHADER_NAME BASIC FRAG

precision mediump float;

#pragma glslify: noise = require('glsl-noise/simplex/3d')

#define PI 3.14159265359
#define TWO_PI 6.28318530718
#define NUM_OCTAVES 5

uniform float u_time;
uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_r;

float circle( vec2 pos, float r ){
    float l = length(pos);

    l = step(r, l);
    l = 1.0 - l;

    return l;
}

float plot(float r, float pct){
  return  smoothstep( pct-0.04, pct, r) -
          smoothstep( pct, pct+0.04, r);
}


float rand(float n){return fract(sin(n) * 43758.5453123);}

float fnoise(float p){
    float fl = floor(p);
  float fc = fract(p);
    return mix(rand(fl), rand(fl + 1.0), fc);
}


float fbm(float x) {
    float v = 0.0;
    float a = 0.5;
    float shift = float(100);
    for (int i = 0; i < NUM_OCTAVES; ++i) {
        v += a * fnoise(x);
        x = x * 2.0 + shift;
        a *= 0.5;
    }
    return v;
}

float poly(vec2 pos, int n){

    float a = atan(pos.x, pos.y) + PI;
    float r = TWO_PI / float(n);

    return cos(floor(0.5 + a/r) * r-a) * length(pos);
}

void main(){

    vec2 uv = gl_FragCoord.xy / u_resolution;
    vec2 pos = uv * 2.0 - 1.0;

    //pos.x *= u_resolution.x / u_resolution.y;

    vec2 mouse = u_mouse * 2.0 - 1.0;
    mouse.y *= -1.0;

    float dist = length(abs(pos - mouse));

    float a = atan(uv.y, uv.x) + noise(vec3(pos, 1.0));

    float ss = sin(u_time) + 1.1 * 20.0;

    float v = noise(vec3(dist, dist, 1.0) * ss * dist * a * 0.25);

    v = smoothstep(0.2, 0.6, v);

    gl_FragColor = vec4(vec3(v), 1.0);
}
