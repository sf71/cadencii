/*
 * SelectedEventChangedEventHandler.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii;

import cadencii.BEventHandler;

public class SelectedEventChangedEventHandler extends BEventHandler{
    public SelectedEventChangedEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }

    public SelectedEventChangedEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
}
#else
using System;
using cadencii;

namespace cadencii {

    public delegate void SelectedEventChangedEventHandler( Object sender, bool foo );

}
#endif