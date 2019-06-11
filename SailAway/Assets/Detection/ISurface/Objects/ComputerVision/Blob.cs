using UnityEngine;
using System.Collections;
using System;

public class Blob : IEquatable<Blob>{

	private int _Id;
	private float _XPosition;
	private float _YPosotion;
	private float _DeltaXPosition;
    private float _DeltaYPosition;
    private float _Acceleration;
    private float _Width;
    private float _Height;
		
	public int Id { get { return _Id; } }
	public float XPosition { get { return _XPosition; } } 
	public float YPosition { get { return _YPosotion; } } 
	public float Width { get { return _Width; } } 
	public float Height { get { return _Height; } }
    public float DeltaX { get { return _DeltaXPosition; } }
    public float DeltaY { get { return _DeltaYPosition; } }
    public float Acceleration { get { return _Acceleration; } }

    public Blob(int id, float x, float y, float dx, float dy, float accel, float wd, float ht)
    {
        _Id = id;
        _XPosition = x;
        _YPosotion = y;
        _DeltaXPosition = dx;
        _DeltaYPosition = dy;
        _Acceleration = accel;
        _Width = wd;
        _Height = ht;
    }

    public bool Equals(Blob other)
    {
        if(other.Id == _Id)
        {
            return true;
        }
        return false;
    }
}
