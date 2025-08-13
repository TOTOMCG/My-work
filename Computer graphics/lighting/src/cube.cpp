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
    unsigned int VAO, VBO, EBO;
    unsigned int texture;
} CubeModel;

typedef struct {
	Vector3 position;
    Matrix rotation;
} GameCube;

GameCube InitGameCube(Vector3 position) {
	GameCube cube;
	cube.position = position;
    cube.rotation = MatrixIdentity();
    
    return cube;
}

CubeModel InitCubeModel(const char* texturePath, bool lightingSource=false) {
    CubeModel cube;
    
    float vertices[] = {
    	-0.5f, -0.5f, -0.5f,  1.0f, 0.0f,  0.0f, 0.0f, -1.0f,
		 0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  0.0f, 0.0f, -1.0f,
		 0.5f,  0.5f, -0.5f,  0.0f, 1.0f,  0.0f, 0.0f, -1.0f,
		-0.5f,  0.5f, -0.5f,  1.0f, 1.0f,  0.0f, 0.0f, -1.0f,
		
		-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,  0.0f, 0.0f, 1.0f,
		 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,  0.0f, 0.0f, 1.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,  0.0f, 0.0f, 1.0f,
		-0.5f,  0.5f,  0.5f,  0.0f, 1.0f,  0.0f, 0.0f, 1.0f,
		
		-0.5f, -0.5f,  0.5f,  1.0f, 0.0f,  -1.0f, 0.0f, 0.0f,
		-0.5f,  0.5f,  0.5f,  1.0f, 1.0f,  -1.0f, 0.0f, 0.0f,
		-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,  -1.0f, 0.0f, 0.0f,
		-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  -1.0f, 0.0f, 0.0f,
		
		 0.5f, -0.5f,  0.5f,  0.0f, 0.0f,  1.0f, 0.0f, 0.0f,
		 0.5f,  0.5f,  0.5f,  0.0f, 1.0f,  1.0f, 0.0f, 0.0f,
		 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,  1.0f, 0.0f, 0.0f,
		 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,  1.0f, 0.0f, 0.0f,
		
		-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  0.0f, -1.0f, 0.0f,
		 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,  0.0f, -1.0f, 0.0f,
		 0.5f, -0.5f,  0.5f,  0.0f, 1.0f,  0.0f, -1.0f, 0.0f,
		-0.5f, -0.5f,  0.5f,  1.0f, 1.0f,  0.0f, -1.0f, 0.0f,
		
		-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,  0.0f, 1.0f, 0.0f,
		 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,  0.0f, 1.0f, 0.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,  0.0f, 1.0f, 0.0f,
		-0.5f,  0.5f,  0.5f,  0.0f, 0.0f,  0.0f, 1.0f, 0.0f
    };
    
    unsigned int indices[] = {
    	0, 2, 1,  2, 0, 3,    // задняя грань
        4, 5, 6,  6, 7, 4,    // передняя грань
        8, 9, 11, 9, 10, 11,  // левая грань
        12, 14, 13, 12, 15, 14, // правая грань
        16, 17, 19, 17, 18, 19, // нижняя грань
        20, 23, 21, 21, 23, 22  // верхняя грань
	};  

    
    glGenVertexArrays(1, &cube.VAO);
    glGenBuffers(1, &cube.VBO);
    glGenBuffers(1, &cube.EBO);
    
    glBindVertexArray(cube.VAO);
    
    glBindBuffer(GL_ARRAY_BUFFER, cube.VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
    
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, cube.EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);
    
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8*sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);
    
    if (lightingSource) {		
		return cube;
    }
    
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 8*sizeof(float), (void*)(3*sizeof(float)));
    glEnableVertexAttribArray(1);
    glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, 8*sizeof(float), (void*)(5*sizeof(float)));
	glEnableVertexAttribArray(2);
    
    Image image = LoadImage(texturePath);
    ImageFlipVertical(&image);
    
    glGenTextures(1, &cube.texture);
    glBindTexture(GL_TEXTURE_2D, cube.texture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, image.width, image.height, 0, GL_RGB, GL_UNSIGNED_BYTE, image.data);
    glGenerateMipmap(GL_TEXTURE_2D);
    
    UnloadImage(image);
    
    return cube;
}

void UpdateGameCube(GameCube* cube) {
    cube->rotation = MatrixRotateY((float)GetTime());
}

void DrawCube(GameCube* gcube, CubeModel* cubem, Matrix view, Matrix projection, Shader shader) {
    glUseProgram(shader.id);

    int modelLoc = GetShaderLocation(shader, "model");
    int viewLoc = GetShaderLocation(shader, "view");
    int projLoc = GetShaderLocation(shader, "projection");
    
    Matrix model = MatrixMultiply(gcube->rotation, MatrixTranslate(gcube->position.x, gcube->position.y, gcube->position.z));
    
    SetShaderValueMatrix(shader, modelLoc, model);
    SetShaderValueMatrix(shader, viewLoc, view);
    SetShaderValueMatrix(shader, projLoc, projection);

    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, cubem->texture);

    glBindVertexArray(cubem->VAO);
    glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
    glBindVertexArray(0);
}

void UnloadCubeModel(CubeModel* cube) {
    glDeleteVertexArrays(1, &cube->VAO);
    glDeleteBuffers(1, &cube->VBO);
    glDeleteBuffers(1, &cube->EBO);
    glDeleteTextures(1, &cube->texture);
}

int main(void) {
    const int screenWidth = 800;
    const int screenHeight = 450;
    
    InitWindow(screenWidth, screenHeight, "a");
    SetTargetFPS(60);

    Shader cubeShader = LoadShader("resources/shaders/shader.vs", "resources/shaders/shader.fs");
    Shader lightShader = LoadShader("resources/shaders/shader.vs", "resources/shaders/lightSource.fs");
    
    Vector3 lightColor = {1.0f, 0.0f, 0.9f};
    Vector3 lightPos = {0.0f, 2.0f, 0.0f};
    SetShaderValue(cubeShader, GetShaderLocation(cubeShader, "lightColor"), &lightColor, SHADER_UNIFORM_VEC3);
    SetShaderValue(cubeShader, GetShaderLocation(cubeShader, "lightPos"), &lightPos, SHADER_UNIFORM_VEC3);
    SetShaderValue(lightShader, GetShaderLocation(lightShader, "lightColor"), &lightColor, SHADER_UNIFORM_VEC3);
    

    Camera camera = {
        .position = { 3.0f, 3.0f, 3.0f },
        .target = { 0.0f, 0.0f, 0.0f },
        .up = { 0.0f, 1.0f, 0.0f },
        .fovy = 45.0f,
        .projection = CAMERA_PERSPECTIVE
    };
    
    SetShaderValue(cubeShader, GetShaderLocation(cubeShader, "viewPos"), &camera.position, SHADER_UNIFORM_VEC3);
    
    CubeModel cubem = InitCubeModel("resources/images/umnyj-chelovek-v-ochkah.jpg", false);

    GameCube gcubes[3];
    gcubes[0] = InitGameCube((Vector3){0.0f, 0.0f, 0.0f});
    gcubes[1] = InitGameCube((Vector3){-1.0f, -1.0f, -1.0f});
    gcubes[2] = InitGameCube((Vector3){1.0f, 1.0f, 1.0f});
    
    GameCube glightCube = InitGameCube(lightPos);
    CubeModel lightCubem = InitCubeModel("resources/images/umnyj-chelovek-v-ochkah.jpg", true);
    
    DisableCursor();
    rlEnableBackfaceCulling();
    
    while (!WindowShouldClose()) {

        UpdateCamera(&camera, CAMERA_FREE);
        
        for (int i = 0; i < 3; i++) {
        	UpdateGameCube(&gcubes[i]);
        }
        
        Matrix matView = GetCameraMatrix(camera);
        Matrix matProjection = MatrixPerspective(camera.fovy*DEG2RAD, (double)screenWidth/screenHeight, 0.01, 1000.0);

        BeginDrawing();
            ClearBackground(WHITE);
            glEnable(GL_DEPTH_TEST);

            for (int i = 0; i < 3; i++) {
                DrawCube(&gcubes[i], &cubem, matView, matProjection, cubeShader);
            }
            
            DrawCube(&glightCube, &lightCubem, matView, matProjection, lightShader);

            DrawGrid(10, 1.0f);
        EndDrawing();
    }

    UnloadCubeModel(&cubem);
    
    UnloadShader(cubeShader);
	UnloadShader(lightShader);
    CloseWindow();
    
    return 0;
}
