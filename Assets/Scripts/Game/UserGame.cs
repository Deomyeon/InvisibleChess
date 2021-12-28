using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using InvisibleChess;
using System.Linq;

public enum Color
{
    White,
    Black
}

public enum PieceSprite
{
    WhitePawn,
    BlackPawn,
    WhiteKnight,
    BlackKnight,
    WhiteBishop,
    BlackBishop,
    WhiteRook,
    BlackRook,
    WhiteQueen,
    BlackQueen,
    WhiteKing,
    BlackKing,
    None
}

public class UserGame : MonoBehaviour
{
    public int turnTime;
    public Text turnTimeText;

    public Color turnColor;
    public Image turnColorImage;
    public Sprite turnWhite;
    public Sprite turnBlack;

    public int moraleWhite;
    public int moraleBlack;

    public Text moraleWhiteText;
    public Text moraleBlackText;

    public Text myNameText;
    public Text rivalNameText;
    public Image myColorImage;
    public Image rivalColorImage;
    public UnityEngine.Color whiteColor;
    public UnityEngine.Color blackColor;

    public readonly int[] boardX = { -477, -340, -206, -70, 66, 201, 337, 473 };
    public readonly int[] boardY = { 474, 339, 203, 67, -68, -203, -340, -475 };

    public GameObject boardObject;
    public GameObject piecesParent;
    public GameObject positionsParent;
    public GameObject blindsParent;
    public ChessPiece[,] board = new ChessPiece[8, 8];
    public GameObject[,] blinds = new GameObject[8, 8];

    public Sprite[] pieces = new Sprite[13];

    public PromotionTab promotionTab;
    public LoadingTab loadingTab;
    public DeadPiece deadPiece;
    public PauseTab pauseTab;
    public ResultTab resultTab;
    public bool moveLock;


    public bool gameEnd;


    public AudioClip backgroundMusic;
    public AudioClip setPieceSound;
    public AudioClip gameSetSound;
    public AudioSource backgroundAudio;
    public AudioSource setPieceAudio;
    public AudioSource gameSetAudio;

    private void Start()
    {
        turnTime = 30;
        moraleWhite = 50;
        moraleBlack = 50;

        turnTimeText.text = turnTime.ToString();
        moraleWhiteText.text = moraleWhite.ToString();
        moraleBlackText.text = moraleBlack.ToString();

        myNameText.text = GameManager.Instance.myName;
        rivalNameText.text = GameManager.Instance.rivalName;

        if (GameManager.Instance.roomRole == RoomRole.White)
        {
            myColorImage.color = whiteColor;
            rivalColorImage.color = blackColor;
        }
        else
        {
            myColorImage.color = blackColor;
            rivalColorImage.color = whiteColor;
        }

        deadPiece.SetColor((GameManager.Instance.roomRole == RoomRole.White) ? Color.Black : Color.White);

        turnColor = Color.White;

        if (turnColor == Color.White)
        {
            turnColorImage.sprite = turnWhite;
        }
        else
        {
            turnColorImage.sprite = turnBlack;
        }

        {
            ChessPiece[] temp = piecesParent.GetComponentsInChildren<ChessPiece>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = temp[i * 8 + j];
                    board[i, j].GetComponent<RectTransform>().localPosition = new Vector2(boardX[(GameManager.Instance.roomRole != RoomRole.White) ? 7 - i : i], boardY[(GameManager.Instance.roomRole == RoomRole.White) ? 7 - j : j]);
                }
            }
        }

        {
            Image[] temp = blindsParent.GetComponentsInChildren<Image>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    blinds[(GameManager.Instance.roomRole == RoomRole.White) ? j : 7 - j, (GameManager.Instance.roomRole == RoomRole.White) ? 7 - i : i] = temp[i * 8 + j].gameObject;
                }
            }
        }

        {
            if (GameManager.Instance.roomRole == RoomRole.White)
            {
                for (int i = 2; i < 5; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        board[j, i].SetData(j, i, PieceType.None, Color.White, 0, pieces[(int)PieceSprite.None], this);
                    }
                }
                for (int i = 5; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        board[j, i].SetData(j, i, PieceType.Blind, Color.White, 0, pieces[(int)PieceSprite.None], this);
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    board[i, 1].SetData(i, 1, PieceType.Pawn, Color.White, 0, pieces[(int)PieceSprite.WhitePawn], this);
                }

                board[0, 0].SetData(0, 0, PieceType.Rook, Color.White, 0, pieces[(int)PieceSprite.WhiteRook], this);
                board[1, 0].SetData(1, 0, PieceType.Knight, Color.White, 0, pieces[(int)PieceSprite.WhiteKnight], this);
                board[2, 0].SetData(2, 0, PieceType.Bishop, Color.White, 0, pieces[(int)PieceSprite.WhiteBishop], this);
                board[3, 0].SetData(3, 0, PieceType.Queen, Color.White, 0, pieces[(int)PieceSprite.WhiteQueen], this);
                board[4, 0].SetData(4, 0, PieceType.King, Color.White, 0, pieces[(int)PieceSprite.WhiteKing], this);
                board[5, 0].SetData(5, 0, PieceType.Bishop, Color.White, 0, pieces[(int)PieceSprite.WhiteBishop], this);
                board[6, 0].SetData(6, 0, PieceType.Knight, Color.White, 0, pieces[(int)PieceSprite.WhiteKnight], this);
                board[7, 0].SetData(7, 0, PieceType.Rook, Color.White, 0, pieces[(int)PieceSprite.WhiteRook], this);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        board[j, i].SetData(j, i, PieceType.Blind, Color.White, 0, pieces[(int)PieceSprite.None], this);
                    }
                }
                for (int i = 3; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        board[j, i].SetData(j, i, PieceType.None, Color.White, 0, pieces[(int)PieceSprite.None], this);
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    board[i, 6].SetData(i, 6, PieceType.Pawn, Color.Black, 0, pieces[(int)PieceSprite.BlackPawn], this);
                }

                board[0, 7].SetData(0, 7, PieceType.Rook, Color.Black, 0, pieces[(int)PieceSprite.BlackRook], this);
                board[1, 7].SetData(1, 7, PieceType.Knight, Color.Black, 0, pieces[(int)PieceSprite.BlackKnight], this);
                board[2, 7].SetData(2, 7, PieceType.Bishop, Color.Black, 0, pieces[(int)PieceSprite.BlackBishop], this);
                board[3, 7].SetData(3, 7, PieceType.Queen, Color.Black, 0, pieces[(int)PieceSprite.BlackQueen], this);
                board[4, 7].SetData(4, 7, PieceType.King, Color.Black, 0, pieces[(int)PieceSprite.BlackKing], this);
                board[5, 7].SetData(5, 7, PieceType.Bishop, Color.Black, 0, pieces[(int)PieceSprite.BlackBishop], this);
                board[6, 7].SetData(6, 7, PieceType.Knight, Color.Black, 0, pieces[(int)PieceSprite.BlackKnight], this);
                board[7, 7].SetData(7, 7, PieceType.Rook, Color.Black, 0, pieces[(int)PieceSprite.BlackRook], this);
            }
        }

        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j].type == PieceType.Blind)
                    {
                        blinds[i, j].SetActive(true);
                    }
                    else
                    {
                        blinds[i, j].SetActive(false);
                    }
                }
            }
        }

        Thread receiveThread = null;
        receiveThread = new Thread(() =>
        {
            System.Net.Sockets.NetworkStream stream = GameManager.Instance.client.GetStream();
            while (!gameEnd && GameManager.Instance.client.Connected)
            {
                while (!gameEnd && !stream.DataAvailable)
                {
                    byte[] buffer = new byte[1024];
                    int dataSize = 0;
                    try
                    {
                        dataSize = stream.Read(buffer, 0, buffer.Length);

                        GameManager.Instance.resultCommandQueue.Enqueue(((string s) =>
                        {
                            string[] ss = s.Split(';');
                            for (int i = 0; i < ss.Length; i++)
                            {
                                string[] ts = ss[i].Split('~');

                                try
                                {
                                    switch (ts[0])
                                    {
                                        case "Turn":
                                            turnColor = (ts[1] == "White") ? Color.White : Color.Black;
                                            if (turnColor == Color.White)
                                            {
                                                turnColorImage.sprite = turnWhite;
                                            }
                                            else
                                            {
                                                turnColorImage.sprite = turnBlack;
                                            }
                                            turnTime = 30;
                                            turnTimeText.text = turnTime.ToString();

                                            for (int j = 0; j < positionsParent.transform.childCount; j++)
                                            {
                                                positionsParent.transform.GetChild(j).gameObject.SetActive(false);
                                            }
                                            promotionTab.ClosePromotionTab();

                                            moveLock = false;

                                            StopAllCoroutines();
                                            StartCoroutine(TimeUpadate());
                                            StartCoroutine(TextScaler());
                                            StartCoroutine(MoveBoard());

                                            break;
                                        case "Time":
                                            turnTime = int.Parse(ts[1]);
                                            turnTimeText.text = turnTime.ToString();
                                            StopAllCoroutines();
                                            StartCoroutine(TimeUpadate());

                                            moveLock = false;

                                            break;
                                        case "End":
                                            gameEnd = true;
                                            StopAllCoroutines();
                                            receiveThread.Abort();
                                            GameManager.Instance.client.Close();
                                            GameManager.Instance.client = null;
                                            GameManager.Instance.resultCommandQueue.Enqueue(((s) =>
                                            {
                                                if (backgroundAudio != null) backgroundAudio.Stop();
                                                if (setPieceAudio != null) setPieceAudio.Stop();
                                                if (gameSetAudio != null) gameSetAudio.Stop();
                                                UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
                                            }, ""));

                                            break;
                                        case "Board":
                                            {
                                                string[] temp = ts[1].Split('#');

                                                for (int j = 0; j < 8; j++)
                                                {
                                                    for (int k = 0; k < 8; k++)
                                                    {
                                                        string[] datas = temp[j * 8 + k].Split('$');
                                                        PieceType type = (PieceType)int.Parse(datas[0]);
                                                        Color color = (Color)int.Parse(datas[1]);
                                                        Sprite sprite = pieces[(int)PieceSprite.None];
                                                        switch (color)
                                                        {
                                                            case Color.White:
                                                                switch (type)
                                                                {
                                                                    case PieceType.Pawn:
                                                                        sprite = pieces[(int)PieceSprite.WhitePawn];
                                                                        break;
                                                                    case PieceType.Knight:
                                                                        sprite = pieces[(int)PieceSprite.WhiteKnight];
                                                                        break;
                                                                    case PieceType.Bishop:
                                                                        sprite = pieces[(int)PieceSprite.WhiteBishop];
                                                                        break;
                                                                    case PieceType.Rook:
                                                                        sprite = pieces[(int)PieceSprite.WhiteRook];
                                                                        break;
                                                                    case PieceType.Queen:
                                                                        sprite = pieces[(int)PieceSprite.WhiteQueen];
                                                                        break;
                                                                    case PieceType.King:
                                                                        sprite = pieces[(int)PieceSprite.WhiteKing];
                                                                        break;
                                                                }
                                                                break;
                                                            case Color.Black:
                                                                switch (type)
                                                                {
                                                                    case PieceType.Pawn:
                                                                        sprite = pieces[(int)PieceSprite.BlackPawn];
                                                                        break;
                                                                    case PieceType.Knight:
                                                                        sprite = pieces[(int)PieceSprite.BlackKnight];
                                                                        break;
                                                                    case PieceType.Bishop:
                                                                        sprite = pieces[(int)PieceSprite.BlackBishop];
                                                                        break;
                                                                    case PieceType.Rook:
                                                                        sprite = pieces[(int)PieceSprite.BlackRook];
                                                                        break;
                                                                    case PieceType.Queen:
                                                                        sprite = pieces[(int)PieceSprite.BlackQueen];
                                                                        break;
                                                                    case PieceType.King:
                                                                        sprite = pieces[(int)PieceSprite.BlackKing];
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        board[j, k].SetData(j, k, type, color, int.Parse(datas[2]), sprite, this);
                                                    }
                                                }

                                                {
                                                    for (int it = 0; it < 8; it++)
                                                    {
                                                        for (int j = 0; j < 8; j++)
                                                        {
                                                            if (board[it, j].type == PieceType.Blind)
                                                            {
                                                                blinds[it, j].SetActive(true);
                                                            }
                                                            else
                                                            {
                                                                blinds[it, j].SetActive(false);
                                                            }
                                                        }
                                                    }
                                                }

                                                setPieceAudio = SoundManager.Instance.PlaySound(setPieceSound, SoundManager.Instance.baseVolume, false);
                                                setPieceAudio.Play();
                                            }

                                            break;
                                        case "GameSet":
                                            gameEnd = true;
                                            gameSetAudio = SoundManager.Instance.PlaySound(gameSetSound, SoundManager.Instance.baseVolume, false);
                                            gameSetAudio.Play();
                                            resultTab.SetResult(((Color)int.Parse(ts[1]) == (GameManager.Instance.roomRole == RoomRole.White ? Color.White : Color.Black)), () =>
                                            {
                                                StopAllCoroutines();
                                                receiveThread.Abort();
                                                GameManager.Instance.client.Close();
                                                GameManager.Instance.client = null;
                                                GameManager.Instance.resultCommandQueue.Enqueue(((s) =>
                                                {
                                                    if (backgroundAudio != null) backgroundAudio.Stop();
                                                    if (setPieceAudio != null) setPieceAudio.Stop();
                                                    if (gameSetAudio != null) gameSetAudio.Stop();
                                                    UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
                                                }, ""));
                                            });
                                            resultTab.OpenResultPanel();

                                            break;

                                        case "Morale":
                                            {
                                                string[] temp = ts[1].Split('#');

                                                moraleWhite = int.Parse(temp[0]);
                                                moraleBlack = int.Parse(temp[1]);
                                                moraleWhiteText.text = moraleWhite.ToString();
                                                moraleBlackText.text = moraleBlack.ToString();
                                                pauseTab.SetMorales((moraleWhite.ToString(), moraleBlack.ToString()));
                                            }
                                            break;
                                        case "Dead":
                                            {
                                                string[] temp = ts[1].Split('#');

                                                Color color = (GameManager.Instance.roomRole == RoomRole.White) ? Color.White : Color.Black;

                                                Color deadColor = (Color)int.Parse(temp[0]);
                                                PieceType deadType = (PieceType)int.Parse(temp[1]);

                                                if (color != deadColor)
                                                {
                                                    deadPiece.AddCount(deadType);
                                                }
                                            }
                                            break;
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }, Encoding.UTF8.GetString(buffer, 0, dataSize)));
                    }
                    catch
                    {

                    }
                }
            }
        });
        receiveThread.Start();

        backgroundAudio = SoundManager.Instance.PlaySound(backgroundMusic, SoundManager.Instance.baseVolume, true);
        backgroundAudio.PlayDelayed(1);
    }

    public IEnumerator TimeUpadate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (turnTime > 0)
            {
                turnTime--;
                turnTimeText.text = turnTime.ToString();
            }
        }
    }

    public IEnumerator TextScaler()
    {
        Vector3 dest = new Vector3(1.5f, 1.5f, 1f);
        bool g = false;
        while (!g)
        {
            if ((turnTimeText.transform.localScale - dest).magnitude < 0.1f)
            {
                g = true;
            }
            else
            {
                turnTimeText.transform.localScale = Vector3.MoveTowards(turnTimeText.transform.localScale, dest, 10 * Time.deltaTime);
            }
            yield return null;
        }
        while (g)
        {
            if ((turnTimeText.transform.localScale - Vector3.one).magnitude < 0.1f)
            {
                g = false;
            }
            else
            {
                turnTimeText.transform.localScale = Vector3.MoveTowards(turnTimeText.transform.localScale, Vector3.one, 5 * Time.deltaTime);
            }
            yield return null;
        }
    }

    public IEnumerator MoveBoard()
    {
        Vector3 source = boardObject.transform.position;
        Vector3 dest = boardObject.transform.position + new Vector3(0, 0.4f, 0);
        bool g = false;
        while (!g)
        {
            if (Mathf.Abs(boardObject.transform.position.y - dest.y) < 0.01f)
            {
                g = true;
            }
            else
            {
                boardObject.transform.position = Vector3.MoveTowards(boardObject.transform.position, dest, 4 * Time.deltaTime);
            }
            yield return null;
        }
        while (g)
        {
            if (Mathf.Abs(boardObject.transform.position.y - source.y) < 0.01f)
            {
                g = false;

            }
            else
            {
                boardObject.transform.position = Vector3.MoveTowards(boardObject.transform.position, source, 2 * Time.deltaTime);
            }
            yield return null;
        }
    }

    public void Exit()
    {
        loadingTab.OpenLoadingPanel();
        GameManager.Instance.CallQuery($"LeaveGame~{GameManager.Instance.Token}#", (s) =>
        {
            gameEnd = true;
            if (GameManager.Instance.client != null)
            {
                GameManager.Instance.client.Close();
                GameManager.Instance.client = null;
            }
            SocketManager<SocketClient>.Instance.End();
            loadingTab.CloseLoadingPanel();
            GameManager.Instance.resultCommandQueue.Enqueue(((s) =>
            {
                if (backgroundAudio != null) backgroundAudio.Stop();
                if (setPieceAudio != null) setPieceAudio.Stop();
                if (gameSetAudio != null) gameSetAudio.Stop();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
            }, ""));
        });
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Exit();
        }
    }
}

