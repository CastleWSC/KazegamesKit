using UnityEngine;
using UnityEditor;
using System.Linq;


namespace KazegamesKit.Editor
{
    public class PhyscisSimulator : EditorWindow
    {
        class SimulatedBody
        {
            public readonly Rigidbody rigidbody;
            readonly Vector3 originPosition;
            readonly Quaternion originRotation;
            readonly Transform transform;

            public SimulatedBody(Rigidbody rigidbody)
            {
                this.rigidbody = rigidbody;
                this.transform = rigidbody.transform;
                this.originPosition = rigidbody.position;
                this.originRotation = rigidbody.rotation;
            }

            public void Reset()
            {
                transform.position = originPosition;
                transform.rotation = originRotation;

                if(rigidbody != null)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }
            }
        }


        [MenuItem("Tools/Kazegames/Physics Simulator")]
        static void CreateWindow()
        {
            GetWindow<PhyscisSimulator>("Physics Simulator");
        }

        public int maxIterations = 1000;
        public Rigidbody[] rigidbodies;

        private SimulatedBody[] _simulatedBodies;

        private void OnGUI()
        {
            SerializedObject so = new SerializedObject(this);

            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(so.FindProperty("rigidbodies"));
            EditorGUILayout.PropertyField(so.FindProperty("maxIterations"));

            EditorGUILayout.Space(10);
            DrawButtons();

            so.ApplyModifiedProperties();
        }

        void DrawButtons()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space(5);
            if (GUILayout.Button("Simulate")) SimulateRigidbodies();
            EditorGUILayout.Space(5);
            if (GUILayout.Button("Reset")) ResetRigidbodies();
            EditorGUILayout.EndVertical();
        }

        private void SimulateRigidbodies()
        {
            if(rigidbodies != null && rigidbodies.Length > 0)
            {
                _simulatedBodies = new SimulatedBody[rigidbodies.Length];
                
                for(int i=0; i<rigidbodies.Length; i++)
                {
                    _simulatedBodies[i] = new SimulatedBody(rigidbodies[i]);
                }
            }


            Physics.autoSimulation = false;
            
            for(int i=0; i<maxIterations; i++)
            {
                Physics.Simulate(Time.fixedDeltaTime);
                if (_simulatedBodies.All(body => body.rigidbody.IsSleeping()))
                {
                    DebugEx.Log(ELogType.Log, $"PhysicsSimulator: Simulation done, iterations= {i}.");
                    break;
                }
            }

            Physics.autoSimulation = true;
        }

        private void ResetRigidbodies()
        {
            if(_simulatedBodies != null && _simulatedBodies.Length > 0)
            {
                foreach (var e in _simulatedBodies)
                    e.Reset();
            }
        }
    }
}
