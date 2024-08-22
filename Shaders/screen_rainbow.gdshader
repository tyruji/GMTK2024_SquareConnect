shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, repeat_disable, filter_nearest;

// --- Uniforms --- /
uniform vec4 color1 : source_color;
uniform float threshold;
uniform float opacity;
uniform float intensity = 1.;
uniform float influence: hint_range(0.0, 1.0, 0.1) = 0.5;
uniform float speed = 100;

const float TWO_PI = 6.28318530718;

// --- Functions --- //
vec4 hsv2rgb(vec3 _c) {
    vec4 _K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 _p = abs(fract(_c.xxx + _K.xyz) * 6.0 - _K.www);
    return vec4( _c.z * mix(_K.xxx, clamp(_p - _K.xxx, 0.0, 1.0), _c.y), 1. );
}

void fragment() {
	vec2 pos = vec2(0.5) - SCREEN_UV;
	float offset = speed * TIME;
	COLOR.rgb = textureLod( SCREEN_TEXTURE, SCREEN_UV, 0.0 ).rgb;
	float distance = length(COLOR - color1);
	vec4 col = influence * hsv2rgb(vec3(((atan(pos.y, pos.x) + radians(offset)) / TWO_PI) + 0.5, length(pos) * 2.0, 1.0));
	if( distance < threshold )
	{
		COLOR = col * intensity;
		COLOR.a = opacity;
	}
}