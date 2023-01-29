using SerializableHelper;
using UnityEditor;
using UnityEngine;

public class FrameComposerEditor : EditorWindow {

    private GameObject selectedGameObject;

    private BodyData bdy;
    private BodyData bdy2;
    private BodyData bdy3;

    private bool applyBdyInUniqueFrame = true;

    private ObjectPointData opoint;
    private ObjectPointData opoint2;
    private ObjectPointData opoint3;

    private InteractionData itr;
    private InteractionData itr2;
    private InteractionData itr3;

    private bool showBdy = false;
    private bool showBdy2 = false;
    private bool showBdy3 = false;
    private bool showItr = false;
    private bool showItr2 = false;
    private bool showItr3 = false;
    private bool showOpoint = false;
    private bool showOpoint2 = false;
    private bool showOpoint3 = false;

    private Vector2 scrollPos;

    [MenuItem("Frame/Composer")]
    public static void Init() {
        var window = GetWindow<FrameComposerEditor>("Frame Composer");
    }

    void OnGUI() {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(600), GUILayout.Height(550));
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        this.selectedGameObject = (GameObject)EditorGUILayout.ObjectField("Object Data Controller", this.selectedGameObject, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        if (!this.selectedGameObject) {
            return;
        }

        if (bdy == null) {
            bdy = new BodyData();
        }

        if (opoint == null) {
            opoint = new ObjectPointData();
        }

        if (itr == null) {
            itr = new InteractionData();
        }

        var dataController = this.selectedGameObject.GetComponent<AbstractDataController>();

        EditorGUILayout.BeginHorizontal();
        showBdy = EditorGUILayout.Toggle("Show bdy1: ", showBdy);
        EditorGUILayout.EndHorizontal();

        if (showBdy) {
            CreateBdyForm(bdy, dataController, dataController.bodysComposer, 1);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        showBdy2 = EditorGUILayout.Toggle("Show bdy2: ", showBdy2);
        EditorGUILayout.EndHorizontal();

        if (showBdy2) {
            CreateBdyForm(bdy2, dataController, dataController.bodysComposer2, 2);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        showBdy3 = EditorGUILayout.Toggle("Show bdy3: ", showBdy3);
        EditorGUILayout.EndHorizontal();

        if (showBdy3) {
            CreateBdyForm(bdy3, dataController, dataController.bodysComposer3, 3);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        showItr = EditorGUILayout.Toggle("Show itr: ", showItr);
        EditorGUILayout.EndHorizontal();

        if (showItr) {
            CreateItrForm(itr, dataController, dataController.interactionsComposer, 1);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        showItr2 = EditorGUILayout.Toggle("Show itr2: ", showItr2);
        EditorGUILayout.EndHorizontal();

        if (showItr2) {
            CreateItrForm(itr2, dataController, dataController.interactionsComposer2, 2);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        showItr3 = EditorGUILayout.Toggle("Show itr3: ", showItr3);
        EditorGUILayout.EndHorizontal();

        if (showItr3) {
            CreateItrForm(itr3, dataController, dataController.interactionsComposer3, 3);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        showOpoint = EditorGUILayout.Toggle("Show opoint: ", showOpoint);
        EditorGUILayout.EndHorizontal();

        if (showOpoint) {
            CreateOpointForm(opoint, dataController, dataController.opointsComposer);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.EndScrollView();

    }

    private void CreateBdyForm(BodyData bdy, AbstractDataController dataController, Map<int, BodyData> composer, int bodyNumber) {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Body");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Load Body")) {
            var hurtboxes = this.selectedGameObject.transform.GetChild(0).gameObject;
            var bdyCurrent = hurtboxes.transform.GetChild(bodyNumber - 1);
            bdy.x = bdyCurrent.localPosition.x;
            bdy.y = bdyCurrent.localPosition.y;
            bdy.z = bdyCurrent.localPosition.z;
            bdy.w = bdyCurrent.localScale.x;
            bdy.h = bdyCurrent.localScale.y;
            bdy.zwidth = bdyCurrent.localScale.z;
            bdy.bodyNumber = bodyNumber;
        }

        EditorGUILayout.BeginHorizontal();
        applyBdyInUniqueFrame = EditorGUILayout.Toggle("Apply bdy in unique frame: ", applyBdyInUniqueFrame);
        EditorGUILayout.EndHorizontal();

        if (applyBdyInUniqueFrame) {
            EditorGUILayout.BeginHorizontal();
            bdy.frameId = EditorGUILayout.IntField("frame id: ", bdy.frameId);
            EditorGUILayout.EndHorizontal();
        } else {
            EditorGUILayout.BeginHorizontal();
            bdy.beginFrameId = EditorGUILayout.IntField("begin frame id: ", bdy.beginFrameId);
            bdy.endFrameId = EditorGUILayout.IntField("end frame id: ", bdy.endFrameId);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        bdy.x = EditorGUILayout.FloatField("x: ", bdy.x);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bdy.y = EditorGUILayout.FloatField("y: ", bdy.y);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bdy.z = EditorGUILayout.FloatField("z: ", bdy.z);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bdy.w = EditorGUILayout.FloatField("w: ", bdy.w);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bdy.h = EditorGUILayout.FloatField("h: ", bdy.h);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bdy.zwidth = EditorGUILayout.FloatField("zwidth: ", bdy.zwidth);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bdy.bodyNumber = EditorGUILayout.IntField("bodyNumber: ", bdy.bodyNumber);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save Body in Frame Composition")) {

            if (composer == null) {
                composer = new Map<int, BodyData>();
            }

            if (applyBdyInUniqueFrame) {
                if (composer.ContainsKey(bdy.frameId)) {
                    var bodyExists = composer[bdy.frameId];
                    bodyExists.x = bdy.x;
                    bodyExists.y = bdy.y;
                    bodyExists.z = bdy.z;
                    bodyExists.w = bdy.w;
                    bodyExists.h = bdy.h;
                    bodyExists.zwidth = bdy.zwidth;
                    bodyExists.bodyNumber = bdy.bodyNumber;

                } else {
                    composer.Add(bdy.frameId, bdy);
                }
            } else {

                if (bdy.beginFrameId < bdy.endFrameId) {

                    for (int i = bdy.beginFrameId; i <= bdy.endFrameId; i++) {
                        bdy.frameId = i;
                        if (composer.ContainsKey(bdy.frameId)) {
                            var bodyExists = composer[bdy.frameId];
                            bodyExists.x = bdy.x;
                            bodyExists.y = bdy.y;
                            bodyExists.z = bdy.z;
                            bodyExists.w = bdy.w;
                            bodyExists.h = bdy.h;
                            bodyExists.zwidth = bdy.zwidth;
                            bodyExists.bodyNumber = bdy.bodyNumber;

                        } else {
                            composer.Add(bdy.frameId, bdy);
                        }
                    }
                    EditorUtility.SetDirty(dataController);
                }

            }
        }
    }

    private void CreateItrForm(InteractionData itr, AbstractDataController dataController, Map<int, InteractionData> composer, int itrNumber) {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Itr");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Load Itr")) {
            var hitboxes = this.selectedGameObject.transform.GetChild(1).gameObject;
            var itrCurrent = hitboxes.transform.GetChild(itrNumber - 1);
            itr.x = itrCurrent.localPosition.x;
            itr.y = itrCurrent.localPosition.y;
            itr.z = itrCurrent.localPosition.z;
            itr.w = itrCurrent.localScale.x;
            itr.h = itrCurrent.localScale.x;
            itr.zwidthz = itrCurrent.localScale.x;
            itr.itrNumber = itrNumber;
        }

        EditorGUILayout.BeginHorizontal();
        itr.frameId = EditorGUILayout.IntField("frame id: ", itr.frameId);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        itr.x = EditorGUILayout.FloatField("x: ", itr.x);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        itr.y = EditorGUILayout.FloatField("y: ", itr.y);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        itr.z = EditorGUILayout.FloatField("z: ", itr.z);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        itr.w = EditorGUILayout.FloatField("h: ", itr.w);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        itr.h = EditorGUILayout.FloatField("w: ", itr.h);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        itr.zwidthz = EditorGUILayout.FloatField("zwidthz: ", itr.zwidthz);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        itr.itrNumber = EditorGUILayout.IntField("itrNumber: ", itr.itrNumber);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save in Frame Composition")) {
            if (composer == null) {
                composer = new Map<int, InteractionData>();
            }

            if (composer.ContainsKey(itr.frameId)) {
                var itrExists = composer[itr.frameId];
                itrExists.x = itr.x;
                itrExists.y = itr.y;
                itrExists.z = itr.z;
                itrExists.w = itr.w;
                itrExists.h = itr.h;
                itrExists.zwidthz = itr.zwidthz;
                itrExists.itrNumber = itr.itrNumber;

            } else {
                composer.Add(itr.frameId, itr);
            }

            EditorUtility.SetDirty(dataController);
        }
    }

    private void CreateOpointForm(ObjectPointData opoint, AbstractDataController dataController, Map<int, ObjectPointData> composer) {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Opoint");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Load Opoint")) {
            var opointCurrent = this.selectedGameObject.transform.GetChild(2).transform;
            opoint.x = opointCurrent.localPosition.x;
            opoint.y = opointCurrent.localPosition.y;
            opoint.z = opointCurrent.localPosition.z;
        }

        EditorGUILayout.BeginHorizontal();
        opoint.frameId = EditorGUILayout.IntField("frame id: ", opoint.frameId);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        opoint.x = EditorGUILayout.FloatField("x: ", opoint.x);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        opoint.y = EditorGUILayout.FloatField("y: ", opoint.y);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        opoint.z = EditorGUILayout.FloatField("z: ", opoint.z);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        opoint.opointNumber = EditorGUILayout.IntField("opointNumber: ", opoint.opointNumber);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save in Frame Composition")) {
            if (composer == null) {
                composer = new Map<int, ObjectPointData>();
            }

            if (composer.ContainsKey(opoint.frameId)) {
                var opointExists = composer[opoint.frameId];
                opointExists.x = opoint.x;
                opointExists.y = opoint.y;
                opointExists.z = opoint.z;
                opointExists.opointNumber = opoint.opointNumber;

            } else {
                composer.Add(opoint.frameId, opoint);
            }

            EditorUtility.SetDirty(dataController);
        }
    }
}
