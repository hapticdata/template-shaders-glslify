precision mediump float;

#define PI 3.14159265359
#define TWO_PI 6.28318530718

varying vec2 vUv;

uniform float u_time;
uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_r;
uniform float u_t;

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


float poly(vec2 pos, int n){

    float a = atan(pos.x, pos.y) + PI;
    float r = TWO_PI / float(n);

    return cos(floor(0.5 + a/r) * r-a) * length(pos);
}

void main(){

    vec2 uv = gl_FragCoord.xy / u_resolution;
    vec2 pos = uv * 2.0 - 1.0;
    //pos.x *= u_resolution.x / u_resolution.y;

    vec3 color = vec3(0.0);


    float d = 0.0;



    //d = 1.0 - (cos(uv.x * TWO_PI) + 1.0) * 0.15;

    d = 1.0 - (sin(uv.y * TWO_PI * 24.0) + 1.0) * 0.5; // + ((sin(fract(u_time * 0.2) * TWO_PI) * uv.x) + 1.0) * 0.5;


    d = step(0.999, d);

    vec2 mouse = u_mouse * 2.0 - 1.0;
    mouse.y *= -1.0;

    d = length(abs(pos - mouse));


    //d = step(0.999, d);

    /*d *= step(0.1, uv.x) * step(uv.x, 0.9);

    d *= step(0.1, uv.y) * step(uv.y, 0.9);*/

    

    color = vec3(d);

    color.r = (sin(u_t * 0.25) + 1.0) / 2.0;





    gl_FragColor = vec4(color, 1.0);
}
