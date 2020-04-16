﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noise;

public class World : MonoBehaviour
{
    [SerializeField] byte[,,] worldData;
    [SerializeField] int worldX = 16;
    [SerializeField] int worldY = 16;
    [SerializeField] int worldZ = 16;
    [SerializeField] GameObject chunk;
    Chunk[,,] chunks;
    [SerializeField] int chunkSize = 16;

    public int ChunkSize
    {
        get
        {
            return chunkSize;
        }
    }
    public Chunk[,,] Chunks
    {
        get
        {
            return chunks;
        }
    }
    public byte[,,] WorldData
    {
        get
        {
            return worldData;
        }
        set
        {
            worldData = value;
        }
    }

    void Start()
    {
        worldData = new byte[worldX, worldY, worldZ];
        for(int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldX; z++)
            {
                int rock = PerlinNoise(x, 0, z, 10f, 3f, 1.2f);
                rock += PerlinNoise(x, 200, z, 20, 8f, 0f) + 10;
                int grass = PerlinNoise(x, 100, z, 50, 30f, 0f) + 1;
                for (int y = 0; y < worldX; y++)
                {
                    if (y <= rock)
                    {
                        worldData[x, y, z] = (byte)TextureType.grass.GetHashCode();
                    }
                    else if (y <= grass)
                    {
                        worldData[x, y, z] = (byte)TextureType.rock.GetHashCode();
                    }
                }
            }
        }
        chunks = new Chunk[Mathf.FloorToInt(worldX / chunkSize), Mathf.FloorToInt(worldY / chunkSize), Mathf.FloorToInt(worldZ / chunkSize)];
        for(int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int y = 0; y < chunks.GetLength(1); y++)
            {
                for (int z = 0; z < chunks.GetLength(2); z++)
                {
                    GameObject newChunk = Instantiate(chunk, new Vector3(x * chunkSize - 0.5f, y * chunkSize + 0.5f, z * chunkSize - 0.5f), new Quaternion(0, 0, 0, 0)) as GameObject;
                    chunks[x, y, z] = newChunk.GetComponent("Chunk") as Chunk;
                    chunks[x, y, z].WorldGO = gameObject;
                    chunks[x, y, z].ChunkSize = chunkSize;
                    chunks[x, y, z].ChunkX = x * chunkSize;
                    chunks[x, y, z].ChunkY = y * chunkSize;
                    chunks[x, y, z].ChunkZ = z * chunkSize;
                }
            }

        }
    }

    void Update()
    {
        
    }
    public int PerlinNoise(int x,int y,int  z,float scale , float height,float power)
    {
        float perlinvalue;
        perlinvalue = Noise.Noise.GetNoise((double)x / scale, (double)y / scale, (double)z / scale);
        perlinvalue *= height;
        if (power != 0)
        {
            perlinvalue = Mathf.Pow(perlinvalue, power);
        }
        return (int)perlinvalue;
    }
    public byte Block(int x,int y,int z)
    {
        if (x >= worldX || x < 0 || y >= worldX || y < 0 || z >= worldX || z < 0)
        {
            return (byte)TextureType.rock.GetHashCode();
        }
        return worldData[x, y, z];
    }
    public void GenerateChunk(int x ,int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            GameObject newChunk = Instantiate(chunk, new Vector3(x * chunkSize - 0.5f, y * chunkSize + 0.5f, z * chunkSize - 0.5f), new Quaternion(0, 0, 0, 0)) as GameObject;
            chunks[x, y, z] = newChunk.GetComponent("Chunk") as Chunk;
            chunks[x, y, z].WorldGO = gameObject;
            chunks[x, y, z].ChunkSize = chunkSize;
            chunks[x, y, z].ChunkX = x * chunkSize;
            chunks[x, y, z].ChunkY = y * chunkSize;
            chunks[x, y, z].ChunkZ = z * chunkSize;
        }
    }
    public void DestroyChunk(int x,int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            Object.Destroy(Chunks[x, y, z].gameObject);
        }
    }
}
