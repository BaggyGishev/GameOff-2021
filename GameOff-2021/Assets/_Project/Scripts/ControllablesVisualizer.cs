using System.Collections;
using System.Collections.Generic;
using Gisha.GameOff_2021.Interactive;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class ControllablesVisualizer : MonoBehaviour
    {
        private static ControllablesVisualizer Instance { get; set; }

        [SerializeField] private GameObject controllableVisualPrefab;

        private List<GameObject> controllableVisualsList = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        public static void SpawnControllableVisuals(Controllable[] controllables)
        {
            for (int i = 0; i < controllables.Length; i++)
            {
                if (controllables[i] == null)
                    continue;

                var element = Instantiate(Instance.controllableVisualPrefab, Instance.transform);
                Instance.StartCoroutine(Instance.UpdateVisualPosition(controllables[i], element.transform));

                Instance.controllableVisualsList.Add(element);
            }
        }

        public static void RemoveControllableUIElements()
        {
            var list = Instance.controllableVisualsList;
            Instance.StopAllCoroutines();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                Destroy(list[i]);
                list.RemoveAt(i);
            }
        }

        private IEnumerator UpdateVisualPosition(Controllable controllable, Transform visual)
        {
            while (true)
            {
                visual.position = controllable.transform.position;
                yield return null;
            }
        }
    }
}