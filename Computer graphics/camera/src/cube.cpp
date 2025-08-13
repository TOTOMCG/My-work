#include "raylib.h"
#include "glad.h"

#include <cstring>

#if defined(PLATFORM_DESKTOP) || defined(PLATFORM_DESKTOP_SDL)
    #define GLSL_VERSION            330
#else
    #define GLSL_VERSION            100
#endif

#include "rlgl.h"
#include "raymath.h"

typedef struct {
    Vector3 position;
    Matrix rotation;
    unsigned int VAO, VBO, EBO;
    Shader shader;
    unsigned int texture;
    float vertices[120];
    unsigned int indices[36];
} Cube;

Cube InitCube(Vector3 position, Shader shader, const char* texturePath) {
    Cube cube;
    cube.position = position;
    cube.rotation = MatrixIdentity();
    cube.shader = shader;
    
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
    
    memcpy(cube.vertices, vertices, sizeof(vertices));
    memcpy(cube.indices, indices, sizeof(indices));
    
    glGenVertexArrays(1, &cube.VAO);
    glGenBuffers(1, &cube.VBO);
    glGenBuffers(1, &cube.EBO);
    
    glBindVertexArray(cube.VAO);
    
    glBindBuffer(GL_ARRAY_BUFFER, cube.VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
    
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, cube.EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);
    
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5*sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);
    
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5*sizeof(float), (void*)(3*sizeof(float)));
    glEnableVertexAttribArray(1);
    
    Image image = LoadImage(texturePath);
    ImageFlipVertical(&image);
    
    glGenTextures(1, &cube.texture);
    glBindTexture(GL_TEXTURE_2D, cube.texture);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, image.width, image.height, 0, GL_RGB, GL_UNSIGNED_BYTE, image.data);
    glGenerateMipmap(GL_TEXTURE_2D);
    
    UnloadImage(image);
    
    return cube;
}

void UpdateCube(Cube* cube) {
    cube->rotation = MatrixRotateY((float)GetTime());
}

void DrawCube(Cube* cube, Matrix view, Matrix projection) {
    glUseProgram(cube->shader.id);

    int modelLoc = GetShaderLocation(cube->shader, "model");
    int viewLoc = GetShaderLocation(cube->shader, "view");
    int projLoc = GetShaderLocation(cube->shader, "projection");
    
    Matrix model = MatrixMultiply(cube->rotation, MatrixTranslate(cube->position.x, cube->position.y, cube->position.z));
    
    SetShaderValueMatrix(cube->shader, modelLoc, model);
    SetShaderValueMatrix(cube->shader, viewLoc, view);
    SetShaderValueMatrix(cube->shader, projLoc, projection);

    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, cube->texture);

    glBindVertexArray(cube->VAO);
    glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
    glBindVertexArray(0);
}

void UnloadCube(Cube* cube) {
    glDeleteVertexArrays(1, &cube->VAO);
    glDeleteBuffers(1, &cube->VBO);
    glDeleteBuffers(1, &cube->EBO);
    glDeleteTextures(1, &cube->texture);
}

int main(void) {
    const int screenWidth = 800;
    const int screenHeight = 450;
    
    InitWindow(screenWidth, screenHeight, "3D Cubes with Classes");
    SetTargetFPS(60);

    Shader shader = LoadShader("resources/shaders/shader.vs", "resources/shaders/shader.fs");

    Camera camera = {
        .position = { 3.0f, 3.0f, 3.0f },
        .target = { 0.0f, 0.0f, 0.0f },
        .up = { 0.0f, 1.0f, 0.0f },
        .fovy = 45.0f,
        .projection = CAMERA_PERSPECTIVE
    };

    Cube cubes[3];
    cubes[0] = InitCube((Vector3){0.0f,0.0f,0.0f}, shader, "resources/images/umnyj-chelovek-v-ochkah.jpg");
    cubes[1] = InitCube((Vector3){1.5f,0.5f,-1.0f}, shader, "resources/images/umnyj-chelovek-v-ochkah.jpg");
    cubes[2] = InitCube((Vector3){-1.5f,-0.5f,-2.0f}, shader, "resources/images/umnyj-chelovek-v-ochkah.jpg");
    
    DisableCursor();
    rlEnableBackfaceCulling();
    
    while (!WindowShouldClose()) {

        UpdateCamera(&camera, CAMERA_FREE);

        for (int i = 0; i < 3; i++) {
            UpdateCube(&cubes[i]);
        }

        Matrix matView = GetCameraMatrix(camera);
        Matrix matProjection = MatrixPerspective(camera.fovy*DEG2RAD, (double)screenWidth/screenHeight, 0.01, 1000.0);

        BeginDrawing();
            ClearBackground(WHITE);
            glEnable(GL_DEPTH_TEST);

            for (int i = 0; i < 3; i++) {
                DrawCube(&cubes[i], matView, matProjection);
            }

            BeginMode3D(camera);
                for (int i = 0; i < 3; i++) {
                    DrawSphereWires(cubes[i].position, 1.0f, 16, 16, RED);
                }
                DrawGrid(10, 1.0f);
            EndMode3D();
            
            DrawFPS(10, 10);
        EndDrawing();
    }

    for (int i = 0; i < 3; i++) {
        UnloadCube(&cubes[i]);
    }
    UnloadShader(shader);
    CloseWindow();
    
    return 0;
}
