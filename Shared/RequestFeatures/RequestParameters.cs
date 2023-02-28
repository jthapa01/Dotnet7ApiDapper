﻿namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    const int maxPazeSize = 50;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get
        {
            return _pageSize;
        }

        set
        {
            _pageSize = (value > maxPazeSize) ? maxPazeSize : value;
        }
    }

    public string? OrderBy { get; set; }
}
