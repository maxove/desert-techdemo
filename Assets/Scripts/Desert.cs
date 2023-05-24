using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Desert : MonoBehaviour
{
    public float perlinScale = 1.0f;
    //Determines the scale of the noise map.
    public float height = 5.0f;
    //Determines the height of the environment.

    public int width = 100;
    public int depth = 100;
    //Width and Depth determine size of the environment.

    public Material material;
    //Initialises the material variable.

    public int spawnObjects;

    public GameObject[] treePrefabs;

    void Start()
    {
        perlinScale = Random.Range(7.0f, 9.0f);
        //Initialises a list that Vector3 values will be stored in.
        List<Vector3> vertices = new List<Vector3>();

        //Initialises a list that integers will be stored in.
        List<int> triangles = new List<int>();

        //Initialises a list that Vector2 values will be stored in.
        List<Vector2> uv = new List<Vector2>();

        //Nested for loop that will generate a mesh. 
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < depth; j++) {
                
                
                //Determines the X and Y coordiantes by using the integers i and j that the for loop iterates through.
                float xCord = (i / (float) width);
                float yCord = (j / (float) depth);

                //Creates the noise value that is stored in the variable y. 
                float y = height * Mathf.PerlinNoise(xCord * perlinScale, yCord * perlinScale);

                //Appends a new Vector3 value to the list vertices.
                vertices.Add(new Vector3(i - width * 0.5f, y, j - depth * 0.5f));

                //Appends a new Vector2 value to the list uv.
                uv.Add(new Vector2((float) i / width, (float) j / depth));

                spawnObjects = Random.Range(1, 200);

                if (spawnObjects == 3){
                    GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                    GameObject tree = Instantiate(prefab, transform);
                    tree.transform.position = new Vector3 (i - (width + Random.Range(0f,5f)) * 0.5f, y, j - depth * 0.5f);
                    tree.transform.rotation = Quaternion.Euler(Random.Range(0, 70f), Random.Range(0, 360f), Random.Range(0, 70f));
                    tree.transform.localScale = Vector3.one * Random.Range(.6f, 1.2f);
                }


                //If i is equal to 0 OR j == 0 continue, skips the first bottom left vertex.
                if(i == 0 || j == 0) continue;

                //Top Right
                triangles.Add(width * i + j);

                //Bottom Right
                triangles.Add(width * i + (j - 1));

                //Bottom Left -- First Triangle
                triangles.Add(width * (i - 1) + (j - 1));

                //Bottom Left
                triangles.Add(width * (i - 1) + (j - 1));

                //Top Left
                triangles.Add(width * (i - 1) + j);

                //Top Right -- Second Triangle
                triangles.Add(width * i + j);

            }
        }

        //Creates a new mesh called mesh
        Mesh mesh = new Mesh();

        //Converts the list vertices to an actual array.
        mesh.vertices = vertices.ToArray();
        
        //Converts the list uv to an array.
        mesh.uv = uv.ToArray();

        //Converts the list triangles to an array.
        mesh.triangles = triangles.ToArray();

        //Recalculate the normals of the mesh so it renders/textures properly.
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        Renderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.lightProbeUsage = LightProbeUsage.Off;
            renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            renderer.material = material;
            meshFilter.mesh = mesh;

    }
}
