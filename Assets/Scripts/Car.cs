namespace Assets.Scripts
{
    public class Car
    {
        private static int idGenerator = 0;

        private static int NextID
        {
            get { return idGenerator++; }
        }
    

        public CarController controller;
        public int nextCheckpoint;
        public int carID;

        public Car(CarController carController)
        {
            controller = carController;
            nextCheckpoint = 0;
            carID = NextID;
            carController.ID = carID;
        }
    }
}