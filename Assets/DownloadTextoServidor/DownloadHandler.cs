using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace DownloadText
{
    public class DownloadHandler : MonoBehaviour
    {
        [SerializeField] private TMP_InputField url;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Button btnDownload;
        [SerializeField] private Button btnDelete;

        private void Awake()
        {
            // Inicia estados de visibilidade dos botoes
            btnDownload.gameObject.SetActive(true);
            btnDelete.gameObject.SetActive(false);
        }

        // Inicia coroutine para baixar o texto
        public void DownloadText()
        {
            StartCoroutine(GetText());
        }

        // https://docs.unity3d.com/Manual/UnityWebRequest-RetrievingTextBinaryData.html
        IEnumerator GetText()
        {
            // Cria uma request com o texto do Input Field
            UnityWebRequest www = UnityWebRequest.Get(url.text);
            yield return www.SendWebRequest();

            // Se nao obteve sucesso
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            // Se conseguiu
            else
            {
                Debug.Log(www.downloadHandler.text);

                // Define texto no label
                label.text = www.downloadHandler.text;

                // Da para pegar o conteudo em binario, se quiser!
                //byte[] results = www.downloadHandler.data;

                // Atualiza interface
                btnDownload.gameObject.SetActive(false);
                btnDelete.gameObject.SetActive(true);
            }

            // Libera memoria da requisicao
            www.Dispose();
        }

        // Delete instancia de objeto
        public void Clear()
        {
            label.text = "?????";

            // Atualiza interface
            btnDownload.gameObject.SetActive(true);
            btnDelete.gameObject.SetActive(false);
        }
    }
}
