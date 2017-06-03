using System;
using UnityEditor;
using UnityEngine;
public enum Tile {Grass,  Water,  Sand,Ground, Tree, enumEnd };

[Serializable]
public class TileDictionary : SerializableDictionary<Tile, GameObject>
{
    public TileDictionary()
    {
        for (Tile tile = Tile.Grass; tile != Tile.enumEnd; tile++)
        {
            Add(tile, null);
        }
    }
}

[Serializable]
public class TileVocabulary : SerializableDictionary<string, Tile>
{
    public TileVocabulary()
    {
        for(Tile tile = Tile.Grass; tile != Tile.enumEnd; tile++)
        {
            Add(tile.ToString(), tile);
        }
    }
}

[CustomPropertyDrawer(typeof(TileDictionary))]
public class TileDictionaryDrawer : DictionaryDrawer<Tile, GameObject> { }

[CustomPropertyDrawer(typeof(TileVocabulary))]
public class TileVocabularyDrawer : DictionaryDrawer<string, Tile> { }

[Serializable]
public class Spritesheet : MonoBehaviour
{
    public TileDictionary tiles;
    public TileVocabulary vocabulary;
    public int tileHeight;
    public int tileWidth;

    public Spritesheet()
    {
        tiles = new TileDictionary();
        vocabulary = new TileVocabulary();
    }

    public GameObject getTileBySymbol(string symbol)
    {
        Tile t;
        if(vocabulary.ContainsKey(symbol))
        {
            t = vocabulary[symbol];
            return tiles[t];
        }
        return null;
    }
}

