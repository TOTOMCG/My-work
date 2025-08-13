Для каждого из проектов

Установить mingw для windows:
https://www.mingw-w64.org/downloads/

Добавить mingw в PATH для корректной работы.

Пример для сборки через CMake для MinGW:
```bash
cd build
cmake -G "MinGW Makefiles" ..
cmake --build .
```

В build должен создаться .exe для демонстрации работы. 
