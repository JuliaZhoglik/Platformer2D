using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
    public float speed = 240.0f;
    public float jumpForce = 12.0f;

    private Rigidbody2D _body;
    private Animator _anim;
    private BoxCollider2D _box;
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>(); // нужно, чтобы к объекту GameObject был прикреплен этот второй компонент
        _anim = GetComponent<Animator>();
        _box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 movement = new Vector2(deltaX, _body.velocity.y); // задаем только горизонтальное смещение, сохраняем заданное вертикальное смещение
        _body.velocity = movement;

        Vector3 max = _box.bounds.max;
        Vector3 min = _box.bounds.min;
        // проверяем значение минимальной Y-координаты коллайдера
        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(min.x, min.y - .2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;
        if (hit != null) // если под персонажем обнаружен коллайдер
        {
            grounded = true;
        }

        _body.gravityScale = grounded && deltaX == 0 ? 0 : 1; // остановка при нахождении на поверхности и в статичном состоянии
        if (grounded && Input.GetKeyDown(KeyCode.Space)) // добавляем в условие для прыжка переменную grounded
        {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        MovingPlatform platform = null;
        if (hit != null)
        {
            platform = hit.GetComponent<MovingPlatform>(); // проверяем, может ли двигаться платформа, находящаяся над персонажем
        }

        if (platform != null)
        {
            transform.parent = platform.transform; // выполняем связывание с платформой
        }
        else
        {
            transform.parent = null; // очищаем переменную parent
        }

        _anim.SetFloat("speed", Mathf.Abs(deltaX)); // скорость больше нуля даже при отрицательных значениях переменной velocity

        Vector3 pScale = Vector3.one; // при нахождение вне движущейся платформы масштаб по умолчанию равен 1

        if (platform != null)
        {
            pScale = platform.transform.localScale;
        }

        if (!Mathf.Approximately(deltaX, 0)) // числа типа float не всегда полностью совпадают, поэтому сравним их методом Approximately
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1); // в процессе движения масштабируем положительную или отрицательную 1 для поворота направо или налево
        }
    }
}
