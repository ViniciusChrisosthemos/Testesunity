using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

namespace DownloadObject
{
    public class DownloadHandler : MonoBehaviour
    {
        [SerializeField] private TMP_InputField url;
        [SerializeField] private Button btnDownload;
        [SerializeField] private Button btnDelete;

        private GameObject instance = null;

        private void Awake()
        {
            // Inicia estados de visibilidade dos botoes
            btnDownload.gameObject.SetActive(true);
            btnDelete.gameObject.SetActive(false);
        }

        // Inicia coroutine para baixar o objeto
        public void DownloadObject()
        {
            StartCoroutine(GetAssetBundle());
        }

        // https://docs.unity3d.com/Manual/UnityWebRequest-DownloadingAssetBundle.html
        IEnumerator GetAssetBundle()
        {
            // Cria uma request com o texto do Input Field
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url.text);
            // Executa a requisicao
            yield return www.SendWebRequest();

            // Se nao obteve sucesso
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Erro ao baixar objeto: " + www.error);
            }
            // Se conseguiu
            else
            {
                Debug.Log("Objeto baixado com sucesso!");

                // Carrega asset bundle
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                // Instancia GameObject do assetbundle
                InstantiateAssetBundle(bundle);

                // Descarrega asset bundle (Necessario!)
                bundle.Unload(false);

                // Atualiza interface
                btnDownload.gameObject.SetActive(false);
                btnDelete.gameObject.SetActive(true);
            }

            // Libera memoria da requisicao
            www.Dispose();
        }

        // Carrega asset mais alto na hierarquia e o instancia como GameObject
        public void InstantiateAssetBundle(AssetBundle bundle)
        {
            GameObject obj = bundle.LoadAsset(bundle.GetAllAssetNames()[0]) as GameObject;

            if (obj != null)
            {
                instance = Instantiate(obj, Vector3.zero, Quaternion.identity);

                Debug.Log("Object criado com sucesso!");
            }
            else
            {
                Debug.Log("Erro ao instanciar objeto!");
            }
        }

        // Delete instancia de objeto
        public void DeleteObject()
        {
            if (instance != null)
                Destroy(instance);


            // Atualiza interface
            btnDownload.gameObject.SetActive(true);
            btnDelete.gameObject.SetActive(false);
        }
    }

}