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
    
    InitWindow(screenWidth, screenHeight, "umnyj-chelovek-v-ochkah");
    
    Shader shader = LoadShader("resources/shaders/shader.vs", "resources/shaders/shader.fs");
    
    Vector3 cubePositions[] = {
		{ 0.0f,  0.0f,  0.0f },
		{ 1.0f,  1.5f, -1.0f },
		{ -1.5f,  -0.5f, -2.0f },
	};
    
    Camera camera = {
        .position = { 3.0f, 3.0f, 3.0f },
        .target = { 0.0f, 0.0f, 0.0f },
        .up = { 0.0f, 1.0f, 0.0f },
        .fovy = 45.0f,
        .projection = CAMERA_PERSPECTIVE
    };
    
    float vertices[] = {
    	-0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
		 0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
		 0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
		-0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
		
		-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
		 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
		-0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
		
		-0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
		-0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
		-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
		-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
		
		 0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
		 0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
		 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
		 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
		
		-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
		 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
		 0.5f, -0.5f,  0.5f,  0.0f, 1.0f,
		-0.5f, -0.5f,  0.5f,  1.0f, 1.0f,
		
		-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
		 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
		-0.5f,  0.5f,  0.5f,  0.0f, 0.0f
    };
    
    unsigned int indices[] = {
    	0, 2, 1,  2, 0, 3,    // задняя грань
        4, 5, 6,  6, 7, 4,    // передняя грань
        8, 9, 11, 9, 10, 11,  // левая грань
        12, 14, 13, 12, 15, 14, // правая грань
        16, 17, 19, 17, 18, 19, // нижняя грань
        20, 23, 21, 21, 23, 22  // верхняя грань
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


    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);

    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(3 * sizeof(float)));
    glEnableVertexAttribArray(1);
    
    unsigned int texture;
    glGenTextures(1, &texture);
    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, texture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    
	Image image = LoadImage("resources/images/umnyj-chelovek-v-ochkah.jpg");
	ImageFlipVertical(&image);
	if (image.data) {
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, image.width, image.height, 0, GL_RGB, GL_UNSIGNED_BYTE, image.data);
		glGenerateMipmap(GL_TEXTURE_2D);
    }
    
    int viewLoc = GetShaderLocation(shader, "view");
    int projLoc = GetShaderLocation(shader, "projection");
    int modelLoc = GetShaderLocation(shader, "model");

    
    SetTargetFPS(60);
    
    rlEnableBackfaceCulling();

    while (!WindowShouldClose())
    {
    	UpdateCamera(&camera, CAMERA_FREE);
 		Matrix rotation = MatrixIdentity();
 		rotation = MatrixRotateY((float)GetTime());   
    
        Matrix matView = GetCameraMatrix(camera);
        Matrix matProjection = MatrixPerspective(camera.fovy*DEG2RAD, 
                                              (double)screenWidth/(double)screenHeight, 
                                              0.01, 1000.0);     
	
		
        SetShaderValueMatrix(shader, viewLoc, matView);
        SetShaderValueMatrix(shader, projLoc, matProjection);                                        
    	
          BeginDrawing();
        
        	ClearBackground(WHITE);
        	rlClearScreenBuffers();
        	
        	glUseProgram(shader.id);
        	
        	glBindTexture(GL_TEXTURE_2D, texture);
        	
        	glEnable(GL_DEPTH_TEST);
        	
        	glBindVertexArray(VAO);
        	
        	for (int i = 0; i < 3; i++)
        	{	
        		Matrix matModel = MatrixMultiply(rotation, MatrixTranslate(cubePositions[i].x, cubePositions[i].y, cubePositions[i].z));        		
		        
		        SetShaderValueMatrix(shader, modelLoc, matModel);
		        
		        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
        	}
        	
        	glBindVertexArray(0);
            
            glUseProgram(0);
            
            
        EndDrawing();
    }
    
    glDeleteVertexArrays(1, &VAO);
    glDeleteBuffers(1, &VBO);
    glDeleteBuffers(1, &EBO);
    UnloadImage(image);
    UnloadShader(shader);

    CloseWindow();
    return 0;
}
