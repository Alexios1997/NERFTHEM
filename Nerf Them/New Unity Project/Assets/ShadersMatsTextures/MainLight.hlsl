#ifdef SHADERGRAPH_PREVIEW
        Direction = float(-0.5,0.5,-0.5);
        Color = float(1,1,1);
        Attenuation = 0.4;
#else
        Light light = GetMainLight();
        Direction = light.direction;
        Attenuation = light.distanceAttenuation;
        Color = light.color;
#endif
