var canvas = document.createElement('canvas'),
    gl = canvas.getContext('webgl'),
    createShader = require('gl-shader'),
    animitter = require('animitter');

var glslify = require('glslify');


var shaderProgram = createShader(gl,
        glslify('../shaders/vert.vs'),
        glslify('../shaders/frag.fs')
    );

var resolution;

function resize(){
    var dpr = Math.max(1, Math.min(2, window.devicePixelRatio || 1));

    canvas.width = window.innerWidth * dpr;
    canvas.height = window.innerHeight * dpr;

    canvas.style.width = window.innerWidth + 'px';
    canvas.style.height = window.innerHeight + 'px';

    resolution = [canvas.width, canvas.height];
    gl.viewport(0, 0, canvas.width * dpr, canvas.height * dpr);
}


var mouse = [0, 0];

document.addEventListener('mousemove', function(evt){
    mouse[0] = evt.clientX / window.innerWidth;
    mouse[1] = evt.clientY / window.innerHeight;
});


document.body.appendChild(canvas);



//basic initialization of webbgl
gl.disable(gl.DEPTH_TEST);
gl.enable(gl.BLEND);
resize();


//vertex buffer
var vert = gl.createBuffer();

gl.bindBuffer(gl.ARRAY_BUFFER, vert);

gl.bufferData(
    gl.ARRAY_BUFFER,
    new Float32Array([
        -1.0, 1.0,
        0.0, -1.0,
        -1.0, 0.0,
        1.0, 1.0,
        0.0, 1.0,
        -1.0, 0.0
    ]),
    gl.STATIC_DRAW
);


//texture buffer
var tex = gl.createBuffer();

gl.bindBuffer(gl.ARRAY_BUFFER, tex);
gl.bufferData(
    gl.ARRAY_BUFFER,
    new Float32Array([
        0, 1,
        0, 0,
        1, 1,
        1, 0
    ]),
    gl.STATIC_DRAW
);


animitter(function(delta, elapsed){

    shaderProgram.bind();
    gl.bindBuffer(gl.ARRAY_BUFFER, vert);
    shaderProgram.attributes.position.pointer();
    //
    gl.bindBuffer(gl.ARRAY_BUFFER, tex);
    //shaderProgram.attributes.uv.pointer();
    //for some reason shader dev tools makes u_time NaN sometimes
    //shaderProgram.uniforms.u_time = shaderProgram.uniforms.u_time || 0.1;
    shaderProgram.uniforms.u_time = elapsed / 1000; //delta / 1000;
    shaderProgram.uniforms.u_t = elapsed / 1000;
    shaderProgram.uniforms.u_resolution = resolution;
    shaderProgram.uniforms.u_mouse = mouse;
    shaderProgram.uniforms.u_r = Math.random();

    gl.drawArrays(gl.TRIANGLE_STRIP, 0, 4);
}).start();


window.addEventListener('resize', resize);
