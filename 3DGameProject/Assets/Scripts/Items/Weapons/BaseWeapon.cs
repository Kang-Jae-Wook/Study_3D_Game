using System.Collections;
using UnityEngine;

namespace KangJaeWook.SurvivalShooter.Weapons
{
   public abstract class BaseWeapon : BaseItem
   {
        [Header("Weapon System", order = 0)]
        [SerializeField] private ParticleSystem m_Particle;     //  격발 파티클
        [SerializeField] private AudioClip m_ShootClip;         //  총 쏠때 나올 사운드
        [SerializeField] private AudioClip m_EmptyAmmoClip;     //  총 비어있을 때 나올 사운드
        [SerializeField] private AudioClip m_ReloadClip;        //  총 재장전 사운드
        [SerializeField] protected Transform m_ShootTransform;  //  총알 생성할 위치

        [Header("Weapon Information", order = 1)]
        public float Damage;            //  총기 데미지
        public int Magazine;            //  총 탄알집 크기
        public float Range;             //  총기 사정거리
        public float FireDelayTime;     //  총기발사 후 딜레이 타임
        public float ReloadTime;        //  재장전 시간

        private float m_CurrentWeaponDelay = 0; //  총기 재장전 중 걸린 시간
        private AudioSource m_AudioSource;      //  총기 사운드 관리 컴포넌트
        private IEnumerator m_ReloadFunc;       //  리로르 함수

        public int CurrentAmmos { get; private set; } = 0;          //  현재 총알 개수
        public bool IsReloading { get => m_ReloadFunc != null; }    //  리로드 중 인가?


        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.clip = m_ShootClip;

            CurrentAmmos = Magazine;
        }

        //  virtual ? 가상
        //  부모의 함수를 사용할 수도 있고 따로 재정의 (override) 해서 사용할 수 있다

        public virtual void Shoot()
        {
            if (IsReloading) return;
            if (m_CurrentWeaponDelay > Time.time) return;

            if(CurrentAmmos == 0)
            {
                m_AudioSource.clip = m_EmptyAmmoClip;
            }
            else
            {
                if(m_Particle != null)
                {
                    var particle = Instantiate(m_Particle, m_ShootTransform);
                    particle.Play();

                    Destroy(particle.gameObject, particle.main.startLifetime.constantMax);
                }

                CurrentAmmos--;
                m_CurrentWeaponDelay = Time.time + FireDelayTime;

                CreateAmmo();
            }

            m_AudioSource.Play();
        }

        public virtual void Reload()
        {
            if (IsReloading) return;

            m_ReloadFunc = UpdateReload();
            StartCoroutine(m_ReloadFunc);

        }

        protected virtual IEnumerator UpdateReload()
        {
            //  총기 재장전 할 때 완료 소리가 마지막에 나야 하므로
            //  총기 재장전 딜레이에서 총기 소리 길이만큼 빼주고 부분 부분 으로 대기해준다
            //  처음엔 총기 재장전 딜레이만큼 기다려준다
            float waitTime = ReloadTime - m_ReloadClip.length;
            yield return new WaitForSeconds(waitTime);

            //  총기 재장전 소리를 실행하고
            //  총기 재장전 소리만큼 기다려 준다
            m_AudioSource.clip = m_ReloadClip;
            m_AudioSource.Play();
            yield return new WaitForSeconds(m_ReloadClip.length);

            //  총기 탄약을 채워주고 소리를 다시 총 소리로 변경
            CurrentAmmos = Magazine;
            m_AudioSource.clip = m_ShootClip;

            m_ReloadFunc = null;

            yield break;
        }

        //  abstract function ?
        //  이 클래스를 상속받는다면 이 함수를 반드시 구현해야 한다
        protected abstract void CreateAmmo();
    }
}
