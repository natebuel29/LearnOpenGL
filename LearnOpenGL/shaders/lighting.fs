#version 330 core
out vec4 FragColor;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    sampler2D emission;

    float shininess;
}; 
  
uniform Material material;

struct Light {
    vec3 position;  
    vec3  direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;


};


// POINT LIGHT
//struct Light {
//    vec3 position;  

//    vec3 ambient;
//    vec3 diffuse;
//    vec3 specular;

//    float constant;
//    float linear;
//    float quadratic;
//};

//DIRECTIONAL LIGHT
//struct Light {
//    vec3 direction; 
//    vec3 ambient;
//    vec3 diffuse;
//    vec3 specular;
//};

uniform Light light;  

in vec3 Normal;  
in vec3 FragPos; 
in vec2 TexCoords;

  
uniform vec3 lightPos; 
uniform vec3 lightColor;
uniform vec3 objectColor;
uniform vec3 viewPos;
uniform float time;



void main()
{
    vec3 lightDir = normalize(light.position - FragPos);

    // ambient
    vec3 ambient = light.ambient * texture(material.diffuse, TexCoords).rgb;
  	
    // diffuse 
    vec3 norm = normalize(Normal);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;  

    
    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));


    vec3 emission = vec3(0.0);
  //  if (texture(material.specular, TexCoords).g == 0.0)   /*rough check for blackbox inside spec texture */
 //   {
 //       vec3 emissionMap = vec3(texture(material.emission, TexCoords + vec2(0.0,time*0.75)));
 //       emission = emissionMap * (sin(time)*0.5+0.5)*2.0;
 //   }

    // spotlight (soft edges)
    float theta = dot(lightDir, normalize(-light.direction)); 
    float epsilon = (light.cutOff - light.outerCutOff);
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    diffuse  *= intensity;
    specular *= intensity;
    
    // attenuation
    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
    ambient  *= attenuation; 
    diffuse   *= attenuation;
    specular *= attenuation;   
        
    vec3 result = ambient + diffuse + specular + emission;
    FragColor = vec4(result, 1.0);
} 