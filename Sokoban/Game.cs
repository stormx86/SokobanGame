namespace Sokoban
{
    class Game
    {
        Cell[,] map;
        Cell[,] top;
        int w, h;
        deShowItem showItem;
        deShowStat showStat;
        Place mouse;
        int placed, totals;
        public Game(deShowItem showItem, deShowStat showStat)
        {
            this.showItem = showItem;
            this.showStat = showStat;
        }

        public bool Init(int levelNr, out int width, out int height)
        {
            LevelFile level = new LevelFile();
            map = level.LoadLevel(levelNr);
            if (map == null)
            {
                width = 0;
                height = 0;
                return false;
            }
                width = w = map.GetLength(0);
                height = h = map.GetLength(1);
                top = new Cell[width, height];
                for (int x = 0; x < w; x++)
                    for (int y = 0; y < h; y++)
                    {
                        switch (map[x, y])
                        { 
                            case Cell.user:
                                            mouse = new Place(x,y);
                                            map[x,y] = Cell.none;
                                            top[x,y] = Cell.user;
                                            break;
                            case Cell.abox:
                                            top[x,y] = Cell.abox;
                                            map[x,y] = Cell.none;
                                            break;
                            case Cell.here:
                                            top[x,y] = Cell.none;
                                            break;
                            case Cell.done:
                                            top[x,y] = Cell.abox;
                                            map[x,y] = Cell.here;
                                            break;
                        }
                    }
            return true;
        }

        public void ShowLevel()
        {
            placed = 0;
            totals = 0;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    ShowMapTop(x, y);
                    if (map[x, y] == Cell.here)
                    {
                        totals++;
                        if (top[x, y] == Cell.abox)
                            placed++;
                    }
                }
            showStat(placed, totals);
        }

        private void ShowMapTop(int x, int y)
        {
            if (top[x,y] == Cell.none)
            showItem(new Place(x, y), map[x, y]);
            else
                if(top[x,y] == Cell.abox && map[x,y] ==Cell.here)
                    showItem(new Place(x, y), Cell.done);
                else
                    showItem(new Place(x, y), top[x, y]);
        }

        public void Step(int sx, int sy)
        {
            Place place = new Place(mouse.x + sx, mouse.y + sy);
            if (!InRange(place))
                return;
            if (top[place.x, place.y] == Cell.none)
            {
                top[mouse.x, mouse.y] = Cell.none;
                ShowMapTop(mouse.x, mouse.y);
                top[place.x, place.y] = Cell.user;
                ShowMapTop(place.x, place.y);
                mouse = place;
            }
            if (top[place.x, place.y] == Cell.abox)
            {
                Place After = new Place(place.x + sx, place.y + sy);
                if (!InRange(After))
                    return;
                if (top[After.x, After.y] != Cell.none)
                    return;

                if (map[place.x, place.y] == Cell.here) placed--;
                if (map[After.x, After.y] == Cell.here) placed++;
                showStat(placed, totals);

                top[mouse.x, mouse.y] = Cell.none;
                ShowMapTop(mouse.x, mouse.y);
                top[place.x, place.y] = Cell.user;
                ShowMapTop(place.x, place.y);

                top[After.x, After.y] = Cell.abox;
                ShowMapTop(After.x, After.y);
                mouse = place;
            }
        }

        public bool InRange(Place place)
        {
            if (place.x < 0 || place.x >= w)
                return false;
            if (place.y < 0 || place.y >= h)
                return false;
            if (map[place.x, place.y] == Cell.none)
                return true;
            if (map[place.x, place.y] == Cell.here)
                return true;
            return false;
        }
    }
}
