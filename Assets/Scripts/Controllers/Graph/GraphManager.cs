using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class GraphManager : MonoBehaviour
{
    public TextAsset tileFile;
    public TextAsset envObjects;
    public Spritesheet spritesheet;

    public Dictionary<GridPosition, Node> nodes;
    public Dictionary<Vector2, TileManager> tiles;
    Node nNull;
    // Use this for initialization
    void Awake()
    {
        GridPosition gNull;
        nNull = new Node();
        gNull.idnode = 999999;
        gNull.x = 999999;
        gNull.y = 999999;
        nNull.setGridPosition(gNull);

        tiles = new Dictionary<Vector2, TileManager>();
        nodes = new Dictionary<GridPosition, Node>();
        loadMap();
        loadGraph();

    }


    void loadMap()
    {
        //Read all ground tiles
        string fileContent = tileFile.text;
        //string[] lines = fileContent.Split('\n');
        string[] lines = fileContent.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        string[] line;
        GameObject tile;
        GameObject newTile;
        int i = 0, j = 0;
        foreach (string l in lines)
        {
            line = l.Split(' ');
            foreach (string symbol in line)
            {
                createTilePosition(symbol, i, j);
                i++;
            }
            j--;
            i = 0;
        }
        //Put enviroment objects
        fileContent = envObjects.text;
        lines = fileContent.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        i = 0;
        j = 0;
        foreach (string l in lines)
        {
            line = l.Split(' ');
            foreach (string symbol in line)
            {
                tile = spritesheet.getTileBySymbol(symbol);
                if (tile != null)
                {
                    newTile = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                }
                i++;
            }
            j--;
            i = 0;
        }
    }

    void loadGraph()
    {
        TileManager d1, up, d2, left, right, d3, down, d4;
        foreach (Vector2 position in tiles.Keys)
        {
            tiles[position].createInternEdges();

            d1 = getNeighborTile(position, TilePos.d1);
            up = getNeighborTile(position, TilePos.up);
            d2 = getNeighborTile(position, TilePos.d2);
            left = getNeighborTile(position, TilePos.left);
            right = getNeighborTile(position, TilePos.right);
            d3 = getNeighborTile(position, TilePos.d3);
            down = getNeighborTile(position, TilePos.down);
            d4 = getNeighborTile(position, TilePos.d4);

            tiles[position].createExternEdges(d1, up, d2, left, right, d3, down, d4);

            foreach (Node node in tiles[position].getNodes())
            {
                nodes.Add(node.getGridPosition(), node);
            }
        }
    }

    public TileManager getTile(Vector2 position)
    {
        if (tiles.ContainsKey(position))
            return tiles[position];
        return null;
    }

    public void createTilePosition(string symbol, int i, int j)
    {
        GameObject tile;
        GameObject newTile;
        tile = spritesheet.getTileBySymbol(symbol);
        if (tile != null)
        {
            //* spritesheet.tileWidth / 100     * spritesheet.tileWidth/100
            newTile = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
            tiles.Add(new Vector2(i, j), newTile.GetComponent<TileManager>());
            newTile.GetComponent<TileManager>().setTilePositionGrid(i, j);
            newTile.transform.parent = transform;

            if ((j % 2 == 0 && i % 2 == 0) || ((j + 1) % 2 == 0 && (i + 1) % 2 == 0))
            {
                Transform children = newTile.transform.FindChild("Sprite");
                children.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
        }
    }

    public void addNewTile(TileManager newTile)
    {
        newTile.createInternEdges();

        //Calculate the position on the graph map
        Vector2 position = newTile.transform.position;
        newTile.setTilePositionGrid(position);
        newTile.transform.parent = transform;
        int i = (int) position.x;
        int j = (int) position.y;
        if((j % 2 == 0 && i % 2 == 0) || ((j + 1) % 2 == 0 && (i + 1) % 2 == 0))
        {
            Transform children = newTile.transform.FindChild("Sprite");
            children.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }

        //Put the new tile on tiles dictionary
        tiles.Add(position, newTile);

        //Get the neighborhorTile
        TileManager d1, up, d2, left, right, d3, down, d4;
        d1 = getNeighborTile(position, TilePos.d1);
        up = getNeighborTile(position, TilePos.up);
        d2 = getNeighborTile(position, TilePos.d2);
        left = getNeighborTile(position, TilePos.left);
        right = getNeighborTile(position, TilePos.right);
        d3 = getNeighborTile(position, TilePos.d3);
        down = getNeighborTile(position, TilePos.down);
        d4 = getNeighborTile(position, TilePos.d4);

        newTile.createExternEdges(d1, up, d2, left, right, d3, down, d4);

        //Put the new nodes on tiles dictionary
        foreach (Node node in newTile.getNodes())
        {
            nodes.Add(node.getGridPosition(), node);
        }
    }

    public void removeTile(GameObject tile)
    {
        TileManager tileManager = tile.GetComponent<TileManager>();

        //Calculate the position on the graph map
        Vector2 position = tile.transform.position;

        foreach (Node node in tileManager.getNodes())
        {
            nodes.Remove(node.getGridPosition());
        }
        tiles.Remove(position);
    }

    public TileManager getNeighborTile(Vector2 tile, TilePos pos)
    {
        Vector2 tilePos = new Vector2(1, 1);
        switch (pos)
        {
            case TilePos.d1:
                tilePos = new Vector2(tile[0] - 1, tile[1] + 1);
                break;
            case TilePos.up:
                tilePos = new Vector2(tile[0], tile[1] + 1);
                break;
            case TilePos.d2:
                tilePos = new Vector2(tile[0] + 1, tile[1] + 1);
                break;
            case TilePos.left:
                tilePos = new Vector2(tile[0] - 1, tile[1]);
                break;
            case TilePos.right:
                tilePos = new Vector2(tile[0] + 1, tile[1]);
                break;
            case TilePos.d3:
                tilePos = new Vector2(tile[0] - 1, tile[1] - 1);
                break;
            case TilePos.down:
                tilePos = new Vector2(tile[0], tile[1] - 1);
                break;
            case TilePos.d4:
                tilePos = new Vector2(tile[0] + 1, tile[1] - 1);
                break;
        }
        if (tiles.ContainsKey(tilePos))
            return tiles[tilePos];
        else
            return null;
    }

    public void removeNode(Node node)
    {
        nodes.Remove(node.getGridPosition());
    }

    public Node foundNearestNode(Vector3 position)
    {
        Node near = null;
        Vector2 pos = new Vector3();
        pos.x = (int)position.x;
        pos.y = (int)position.y;
        float distance = Mathf.Infinity;

        foreach (Node n in this.nodes.Values)
        {
            if (!n.active)
                continue;
            Vector3 diff = n.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                near = n;
                distance = curDistance;
            }
        }
        return near;
    }

    #region AStar
    public List<Node> AStar(Node startNode, Node endNode)
    {
        /*if self.heuristic == None:
        self.heuristic = self.manhattanDistance
        closed = []
        openList = [start]
        cameFrom = { }
        gScore = { }
        fScore = { }
        gScore[start] = 0
        fScore[start] = self.heuristic(start, goal)
        fScore[-1] = 99999999*/
        List<Node> closed, openList;
        Dictionary<Node, Node> cameFrom;
        Dictionary<Node, float> fScore, gScore;
        Node current = null;
        float tentative;

        fScore = new Dictionary<Node, float>();
        gScore = new Dictionary<Node, float>();
        closed = new List<Node>();
        openList = new List<Node>();
        openList.Add(startNode);
        cameFrom = new Dictionary<Node, Node>();
        gScore[startNode] = 0;
        fScore[startNode] = heuristic(startNode, endNode);
        fScore[nNull] = 9999999;

        /*while (openList):
        current = self.chooseDestiny(openList, fScore)
        if (current == goal):
            return self.reconstructPath(cameFrom, goal)*/
        if (!endNode.active)
            return null;

        while (openList.Count > 0)
        {

            current = chooseDestiny(openList, fScore);
            if (current == endNode)
                return reconstructPath(cameFrom, endNode);


            /*openList.remove(current)
            closed.append(current)
            neighborVertices = self.graph[current]*/
            openList.Remove(current);
            closed.Add(current);

            if (!current.active)
            {
                continue;
            }
            
            /*openList.remove(current)
            closed.append(current)
            neighborVertices = self.graph[current]
            for n in neighborVertices:
                    if (n in closed):
                            continue
                    tentative = gScore[current] + 1*/

            foreach (Node neighbor in current.getNeighbors())
            {
                if (!neighbor.active || closed.Contains(neighbor))
                {
                    continue;
                }
                tentative = gScore[current] + 1;
                /*if (not(n in openList) or tentative < gScore[n]):
                           cameFrom[n] = current
                           gScore[n] = tentative
                           fScore[n] = gScore[n] + self.heuristic(n, goal)
                           if (not(n in openList)):
                                   openList = openList + [n] */
                if (!openList.Contains(neighbor) || tentative < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative;
                    fScore[neighbor] = gScore[neighbor] + heuristic(neighbor, endNode);
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }
        /*if (current == goal):
            return self.reconstructPath(cameFrom, goal)
          return False;*/
        if (current == endNode)
            return reconstructPath(cameFrom, endNode);
        return null;

    }

    private float heuristic(Node start, Node end)
    {
        if (start == null || end == null)
            Debug.Log("QUE PORRA É ESSA???");
        //ManhattanDistance for the close ones
        return 0.5f * Vector3.Distance(start.transform.position, end.transform.position) * start.getWeight() * end.getWeight();
    }

    private Node chooseDestiny(List<Node> openList, Dictionary<Node, float> fScore)
    {
        /*destiny = -1
        for d in openList:
        if (fScore[destiny] > fScore[d]):
          destiny = d
        return destiny*/
        Node destiny = nNull;
        foreach (Node node in openList)
        {
            if (fScore[destiny] > fScore[node])
                destiny = node;
        }
        return destiny;
    }

    private List<Node> reconstructPath(Dictionary<Node, Node> cameFrom, Node currentNode)
    {
        /*if (currentNode in cameFrom):
            p = self.reconstructPath(cameFrom, cameFrom[currentNode])
            return p + [currentNode]
        else:
            return [(currentNode)]*/
        List<Node> path = new List<Node>();
        if (cameFrom.ContainsKey(currentNode))
            path = reconstructPath(cameFrom, cameFrom[currentNode]);
        path.Add(currentNode);
        return path;
    }

    #endregion

}
