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
    
    Shader shader = LoadShader("resources/shaders/shader.vs", "resources/shaders/shader.fs");
    
    float vertices[] = {
    0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,
    -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,
     0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f
    };
  
    unsigned int VBO;
    unsigned int VAO;
    
    glGenBuffers(1, &VBO);
    glGenVertexArrays(1, &VAO);
     
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBindVertexArray(VAO);
    
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
    
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);
	
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3* sizeof(float)));
	glEnableVertexAttribArray(1);
	
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
        	
        	glDrawArrays(GL_TRIANGLES, 0, 3);
        	
        	glBindVertexArray(0);
            
            glUseProgram(0);
            
        EndDrawing();
    }
    
    glDeleteVertexArrays(1, &VAO);
    glDeleteBuffers(1, &VBO);
    UnloadShader(shader);

    CloseWindow();
    return 0;
}
