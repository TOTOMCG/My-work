cmake_minimum_required(VERSION 3.10)
project(CG)

set(CMAKE_CXX_STANDARD 17)

file(COPY resources/ DESTINATION ${CMAKE_BINARY_DIR}/resources)

set(RAYLIB_INCLUDE_DIR "${CMAKE_SOURCE_DIR}/include")
set(RAYLIB_LIBRARY "${CMAKE_SOURCE_DIR}/lib/libraylib.a")
set(GLAD_LIBRARY "${CMAKE_SOURCE_DIR}/lib/libglad.a")


set(CMAKE_SYSTEM_NAME Windows)
set(CMAKE_C_COMPILER x86_64-w64-mingw32-gcc)
set(CMAKE_CXX_COMPILER x86_64-w64-mingw32-g++)

add_executable(plain_texture src/plain_texture.cpp)
add_executable(texture45 src/texture45.cpp)
add_executable(2texture src/2texture.cpp)
add_executable(rotate src/rotate.cpp)

target_include_directories(plain_texture PRIVATE 
    ${RAYLIB_INCLUDE_DIR}
)


target_compile_definitions(plain_texture PRIVATE 
    PLATFORM_DESKTOP
    GRAPHICS_API_OPENGL_33
)

target_link_libraries(plain_texture 
    ${RAYLIB_LIBRARY}
    ${GLAD_LIBRARY}
    opengl32 
    gdi32 
    winmm 
    user32
    shell32
)

target_include_directories(texture45 PRIVATE 
    ${RAYLIB_INCLUDE_DIR}
)


target_compile_definitions(texture45 PRIVATE 
    PLATFORM_DESKTOP
    GRAPHICS_API_OPENGL_33
)

target_link_libraries(texture45 
    ${RAYLIB_LIBRARY}
    ${GLAD_LIBRARY}
    opengl32 
    gdi32 
    winmm 
    user32
    shell32
)

target_include_directories(2texture PRIVATE 
    ${RAYLIB_INCLUDE_DIR}
)


target_compile_definitions(2texture PRIVATE 
    PLATFORM_DESKTOP
    GRAPHICS_API_OPENGL_33
)

target_link_libraries(2texture
    ${RAYLIB_LIBRARY}
    ${GLAD_LIBRARY}
    opengl32 
    gdi32 
    winmm 
    user32
    shell32
)


target_include_directories(rotate PRIVATE 
    ${RAYLIB_INCLUDE_DIR}
)


target_compile_definitions(rotate PRIVATE 
    PLATFORM_DESKTOP
    GRAPHICS_API_OPENGL_33
)

target_link_libraries(rotate
    ${RAYLIB_LIBRARY}
    ${GLAD_LIBRARY}
    opengl32 
    gdi32 
    winmm 
    user32
    shell32
)

