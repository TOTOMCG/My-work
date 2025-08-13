#include "raylib.h"
#include "glad.h"

#if defined(PLATFORM_DESKTOP) || defined(PLATFORM_DESKTOP_SDL)
    #define GLSL_VERSION            330
#else   // PLATFORM_ANDROID, PLATFORM_WEB
    #define GLSL_VERSION            100
#endif

#include "rlgl.h"
#include "raymath.h"

int main(void)
{
    const int screenWidth = 800;
    const int screenHeight = 450;
    
    InitWindow(screenWidth, screenHeight, "raylib - point particles");
    
    Shader shader = LoadShader("resources/shaders/shader1.vs", "resources/shaders/shader1.fs");
    
    float vertices[] = {
     0.5f,  0.5f, 0.0f,
     0.5f, -0.5f, 0.0f,
    -0.5f, -0.5f, 0.0f,
    -0.5f,  0.5f, 0.0f
    };
    
    unsigned int indices[] = {
    0, 1, 3,
    1, 2, 3
};  
  
    unsigned int VBO, VAO, EBO;
    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO);
    glGenBuffers(1, &EBO);
    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);


    glBindBuffer(GL_ARRAY_BUFFER, 0); 

    glBindVertexArray(0); 
    
    SetTargetFPS(60);

    rlDisableBackfaceCulling();

    while (!WindowShouldClose())
    {
        BeginDrawing();
        
        	ClearBackground(WHITE);
        	
        	rlDrawRenderBatchActive();
        	
        	glUseProgram(shader.id);
        	
        	glBindVertexArray(VAO);

                double  timeValue = GetTime();
                float greenValue = static_cast<float>(sin(timeValue) / 2.0 + 0.5);
                int vertexColorLocation = glGetUniformLocation(shader.id, "ourColor");
                glUniform4f(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);
        	
        	glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
        	
        	glBindVertexArray(0);
            
            glUseProgram(0);
            
        EndDrawing();
    }
    
    glDeleteVertexArrays(1, &VAO);
    glDeleteBuffers(1, &VBO);
    glDeleteBuffers(1, &EBO);
    UnloadShader(shader);

    CloseWindow();
    return 0;
}
