namespace Assets.Scripts
{
    public class Car
    {
        private static int _idGenerator = 0;

        private static int NextId
        {
            get { return _idGenerator++; }
        }

        public CarController Controller;
        public int NextCheckpoint;

        public Car(CarController carController)
        {
            Controller = carController;
            NextCheckpoint = 0;
            carController.ID = NextId;
        }
    }
}