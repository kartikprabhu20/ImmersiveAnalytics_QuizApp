using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotLoader : IPlotLoad
{ 
    private GameObject settingsManager;
    private GameObject scatterPlotManager;

    public PlotLoader(GameObject settingsManager, GameObject scatterPlotManager)
    {
        this.settingsManager = settingsManager;
        this.scatterPlotManager = scatterPlotManager;
    }

    public void callback()
    {
        //settingsManager.GetComponent<SettingsManager>().setScatterPlotGenerator(scatterPlotManager.GetComponent<ScatterplotGenerator>());
        //settingsManager.GetComponent<SettingsManager>().initSettings();
    }

}
