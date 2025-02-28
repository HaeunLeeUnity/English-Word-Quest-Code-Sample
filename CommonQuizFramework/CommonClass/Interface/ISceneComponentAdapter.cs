namespace CommonQuizFramework.CommonClass
{
    // 특정 Scene에 종속된 Component를 참조하는 클래스는 해당 인터페이스를 상속하고 참조를 제거하는 함수를 구현해야한다.
    public interface ISceneComponentAdapter
    {
        public void DisposeSceneComponent();
    }
}