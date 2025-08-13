using UnityEngine;

public enum MeshType
{
    Square,
    Circle,
    Parallelepiped,
    Sphere,
    Wave,
    Slope,
    Cone
}

public class MeshGenerationScript : MonoBehaviour
{
    [Header("Параметры меша")]
    public MeshType meshType;
    
    [Header("Параметры окружности")]
    public float innerRadius = 0.5f;
    public float outerRadius = 1f;
    [Range(3, 256)] public int circleSegments = 64;

    [Header("Параметры параллелепипеда")] 
    public float height = 1f;
    public float depth = 1f;
    public float width = 1f;

    [Header("Параметры сферы")] 
    public float radius = 1f;
    [Range(16, 512)] public int sphereSegments = 128;
    
    [Header("Параметры волны")]
    public float waveSize = 1f;
    public float waveLength = 1f;
    public float waveDepth = 1f;
    public float waveAmplitude = 1f;
    [Range(4, 128)] public int waveSegments = 128;
    
    [Header("Параметры горки")]
    public float slopeLength = 1f;
    public float slopeDepth = 1f;
    [Range(-180, 180)] public float slopeAngle = 45f;

    [Header("Параметры конуса")] 
    public float coneHeight = 3f;
    public float coneRadius = 1f;
    [Range(16, 128)] public int coneSegments = 128;
    
    void Start()
    {
        if (meshType == MeshType.Square)
        {
            GenerateSquare();
        } else if (meshType == MeshType.Circle)
        {
            GenerateCircle();
        } else if (meshType == MeshType.Parallelepiped)
        {
            GenerateParallelepiped();
        } else if (meshType == MeshType.Sphere)
        {
            GenerateSphere();
        } else if (meshType == MeshType.Wave)
        {
            GenerateWave();
        } else if (meshType == MeshType.Slope)
        {
            GenerateSlope();
        } else if (meshType == MeshType.Cone)
        {
            GenerateCone();
        }
    }

    void GenerateSquare()
    {
        Mesh mesh = new Mesh();

        // Вершины (4 точки для квадрата)
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0), // Внизу слева
            new Vector3(1, 0, 0), // Внизу справа
            new Vector3(0, 1, 0), // Вверху слева
            new Vector3(1, 1, 0)  // Вверху справа
        };

        // Порядок соединения вершин (две треугольные грани)
        int[] triangles = new int[]
        {
            0, 2, 1, // Первый треугольник
            2, 3, 1  // Второй треугольник
        };

        // Записываем в mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
        
        // Назначаем mesh объекту
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateCircle()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Circle";

        Vector3[] vertices = new Vector3[circleSegments * 2];
        int[] triangles = new int[circleSegments * 6];

        float angleStep = 2 * Mathf.PI / circleSegments;

        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < circleSegments; i++)
        {
            float angle = i * angleStep;

            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            // Внешняя и внутренняя точки
            Vector3 outer = new Vector3(cos * outerRadius, 0, sin * outerRadius);
            Vector3 inner = new Vector3(cos * innerRadius, 0, sin * innerRadius);

            vertices[vertIndex] = outer;
            vertices[vertIndex + 1] = inner;

            int nextIndex = (i + 1) % circleSegments * 2;

            // Первый треугольник
            triangles[triIndex++] = vertIndex;
            triangles[triIndex++] = nextIndex;
            triangles[triIndex++] = vertIndex + 1;

            // Второй треугольник
            triangles[triIndex++] = vertIndex + 1;
            triangles[triIndex++] = nextIndex;
            triangles[triIndex++] = nextIndex + 1;

            vertIndex += 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateParallelepiped()
    {
        Mesh mesh = new Mesh();
         
        Vector3[] vertices =
        {
            // Передняя грань
            new (0, 0, 0), // Низ слева 0
            new (width, 0, 0), // Низ справа 1
            new (width, height, 0), // Верх справа 2
            new (0, height, 0), // Верх слева 3
            
            // Задняя грань
            new (0, 0, depth), // Аналогичные точки, но сзади 4
            new (width, 0, depth), // 5
            new (width, height, depth), // 6
            new (0, height, depth) // 7
        };
        
        int[] triangles =
        {
            0, 2, 1,
            0, 3, 2,
            
            5, 6, 4,
            4, 6, 7,
            
            3, 6, 2,
            3, 7, 6,
            
            0, 1, 5,
            0, 5, 4,
            
            0, 7, 3,
            0, 4, 7,
            
            1, 2, 6,
            1, 6, 5
        };
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
        
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    void GenerateSphere()
    {
        Mesh mesh = new Mesh();
        
        Vector3[] vertices = new Vector3[(sphereSegments + 1) * (sphereSegments + 1)]; // Разбиваем на долготу и широту (их количество одинаково)
        int[] triangles = new int[sphereSegments * sphereSegments * 6];

        int triIndex = 0;

        float vStep = Mathf.PI / sphereSegments;
        float hStep = 2 * Mathf.PI / sphereSegments;
        
        for (int y = 0; y <= sphereSegments; y++) // Построим в сферических координатах
        {
            float verticalAngle = y * vStep;
            
            for (int x = 0; x <= sphereSegments; x++)
            {
                float horizontalAngle = x * hStep;
                
                vertices[y * (sphereSegments + 1) + x] = new Vector3( // Переводим в декартовы
                    radius * Mathf.Sin(verticalAngle) * Mathf.Cos(horizontalAngle),
                    radius * Mathf.Cos(verticalAngle),
                    radius * Mathf.Sin(verticalAngle) * Mathf.Sin(horizontalAngle)
                );

                if (y < sphereSegments && x < sphereSegments)
                {
                    int vertIndex = y * (sphereSegments + 1) + x;
                    int nextIndex = vertIndex + sphereSegments + 1;

                    // Первый треугольник
                    triangles[triIndex++] = vertIndex;
                    triangles[triIndex++] = nextIndex + 1;
                    triangles[triIndex++] = vertIndex + 1;

                    // Второй треугольник
                    triangles[triIndex++] = vertIndex;
                    triangles[triIndex++] = nextIndex;
                    triangles[triIndex++] = nextIndex + 1;
                }
            }
        }
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
        
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateWave()
    {
        Mesh mesh = new Mesh();
        
        Vector3[] vertices = new Vector3[(waveSegments + 1) * 4];
        int[] triangles = new int[waveSegments * 12];
        
        float step = waveSize / waveSegments;
        
        float xPos = 0f;
        float yPos;
        
        int vertIndex = 0;
        int triIndex = 0;
        
        int offset = (waveSegments+1)*2;

        for (int i = 0; i <= waveSegments; i++)
        {
            xPos += step;
            yPos = waveAmplitude * Mathf.Sin(xPos / waveLength);
            
            vertices[vertIndex] = new Vector3(xPos, yPos, 0);
            vertices[vertIndex + 1] = new Vector3(xPos, yPos, waveDepth);
            
            vertices[offset + vertIndex] = new Vector3(xPos, yPos, 0);
            vertices[offset + vertIndex + 1] = new Vector3(xPos, yPos, waveDepth);
            
            if (i < waveSegments)
            {
                triangles[triIndex++] = vertIndex; // По часовой
                triangles[triIndex++] = vertIndex + 1;
                triangles[triIndex++] = vertIndex + 3;
                
                triangles[triIndex++] = offset + vertIndex; // Против часовой
                triangles[triIndex++] = offset + vertIndex + 3;
                triangles[triIndex++] = offset + vertIndex + 1;
                
                triangles[triIndex++] = vertIndex; // По часовой
                triangles[triIndex++] = vertIndex + 3;
                triangles[triIndex++] = vertIndex + 2;
                
                triangles[triIndex++] = offset + vertIndex; // Против часовой
                triangles[triIndex++] = offset + vertIndex + 2;
                triangles[triIndex++] = offset + vertIndex + 3;
            }
            
            vertIndex += 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateSlope()
    {
        Mesh mesh = new Mesh();
        
        Vector3[] vertices =
        {
            new (0, 0, 0),
            new (slopeLength, 0, 0),
            new (slopeLength, Mathf.Tan(slopeAngle * Mathf.PI / 180) * slopeLength, 0),
            
            new (0, 0, slopeDepth),
            new (slopeLength, 0, slopeDepth),
            new (slopeLength, Mathf.Tan(slopeAngle * Mathf.PI / 180) * slopeLength, slopeDepth)
        };
        
        int[] triangles =
        {
            0, 2, 1,
            
            4, 5, 3,
            
            0, 5, 2,
            0, 3, 5,
            
            0, 1, 4,
            0, 4, 3,
            
            1, 5, 4,
            1, 2, 5,
        };
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    void GenerateCone()
{
    Mesh mesh = new Mesh();
    
    Vector3[] vertices = new Vector3[coneSegments + 2];
    int[] triangles = new int[coneSegments * 6];

    vertices[0] = Vector3.up * height; // Вершина конуса
    
    vertices[1] = new (0, 0, 0); // Центр основания
    
    float step = 2 * Mathf.PI / coneSegments;
    
    int triIndex = 0;
    
    for (int i = 0; i < coneSegments; i++)
    {
        float angle = i * step;
        
        vertices[i + 2] = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

        
        int vertIndex = i + 2;
        int nextIndex = (i + 1) % coneSegments + 2;
        
        triangles[triIndex++] = 0; // Боковые треугольники
        triangles[triIndex++] = nextIndex;
        triangles[triIndex++] = vertIndex;
            
        triangles[triIndex++] = 1; // Треугольник основания
        triangles[triIndex++] = vertIndex;
        triangles[triIndex++] = nextIndex;
        
    }
    
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    
    mesh.RecalculateNormals();
    
    GetComponent<MeshFilter>().mesh = mesh;
    }
}
