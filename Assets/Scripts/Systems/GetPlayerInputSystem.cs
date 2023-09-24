using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace TMG.Shooter
{
    
    public partial class GetPlayerInputSystem : SystemBase //La clase es un sistema
                                                            //Un comportamiento de ciclo de vida, Start-Update-Destroy
    {
        private PlayerInputKeys _playerInputKeys;
        private Camera _mainCamera;

        protected override void OnStartRunning()
        {
            _playerInputKeys = GetSingleton<PlayerInputKeys>();
            _mainCamera = Camera.main;

        }
            
        protected override void OnUpdate()  //Es necesario en clase SystemBase
                                            //Es lo que hace la transformacion de datos en el juego
        { 
            var newPlayerInput = GetPlayerMoveInput();
            SetSingleton(newPlayerInput);

            var curWorldMousePos = GetWorldMousePosition();
            SetSingleton(curWorldMousePos);
        }
        private PlayerMoveInput GetPlayerMoveInput()
        {
            var horizontalMovement = 0f;
            if(Input.GetKey(_playerInputKeys.RightKey) )
            {
                horizontalMovement = 1f;

            }else if (Input.GetKey(_playerInputKeys.LeftKey))
            {
                horizontalMovement = -1f;

            }

            var verticalMovement = 0f; 
            if (Input.GetKey(_playerInputKeys.UpKey))
            {
                verticalMovement= 1f;
            }else if (Input.GetKey(_playerInputKeys.DownKey))
            {
                verticalMovement = -1f;
            }

            var playerMovement = new float3
            {
                x = horizontalMovement,
                y = 0f,
                z = verticalMovement
            };

            if (math.lengthsq(playerMovement) > 1f)
            {
                playerMovement = math.normalize(playerMovement);
            }

            return new PlayerMoveInput
            {
                Value = playerMovement
            };
        }

        private WorldMousePosition GetWorldMousePosition()
        {
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.z);
            var worldMousePos = _mainCamera.ScreenToWorldPoint(mousePos);

            return new WorldMousePosition
            {
                Value = worldMousePos
            };
        }
           
    }
}
