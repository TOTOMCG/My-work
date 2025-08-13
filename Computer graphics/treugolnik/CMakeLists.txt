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

add_executable(triangle src/gr_triangle.cpp)
add_executable(square src/square.cpp)

target_include_directories(triangle PRIVATE 
    ${RAYLIB_INCLUDE_DIR}
)


target_compile_definitions(triangle PRIVATE 
    PLATFORM_DESKTOP
    GRAPHICS_API_OPENGL_33
)

target_link_libraries(triangle 
    ${RAYLIB_LIBRARY}
    ${GLAD_LIBRARY}
    opengl32 
    gdi32 
    winmm 
    user32
    shell32
)

target_include_directories(square PRIVATE 
    ${RAYLIB_INCLUDE_DIR}
)


target_compile_definitions(square PRIVATE 
    PLATFORM_DESKTOP
    GRAPHICS_API_OPENGL_33
)

target_link_libraries(square 
    ${RAYLIB_LIBRARY}
    ${GLAD_LIBRARY}
    opengl32 
    gdi32 
    winmm 
    user32
    shell32
)
