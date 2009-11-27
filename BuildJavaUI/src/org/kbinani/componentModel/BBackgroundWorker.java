package org.kbinani.componentModel;

import org.kbinani.BEvent;

public class BBackgroundWorker{
    public BEvent<BDoWorkEventHandler> doWorkEvent = new BEvent<BDoWorkEventHandler>();
    public BEvent<BProgressChangedEventHandler> progressChangedEvent = new BEvent<BProgressChangedEventHandler>();
    public BEvent<BRunWorkerCompletedEventHandler> runWorkerCompletedEvent = new BEvent<BRunWorkerCompletedEventHandler>();
    private WorkerRunner m_runner = null;

    class WorkerRunner implements Runnable{
        private BDoWorkEventArgs m_arg = null;
        private BEvent<BDoWorkEventHandler> m_delegate = null;
        
        public WorkerRunner( BEvent<BDoWorkEventHandler> delegate, Object argument ){
            m_delegate = delegate;
            BDoWorkEventArgs m_arg = new BDoWorkEventArgs( argument );
        }

        public void run(){
            try{
                m_delegate.raise( m_arg );
                BRunWorkerCompletedEventArgs e = new BRunWorkerCompletedEventArgs( null, null, false );
                runWorkerCompletedEvent.raise( e );
            }catch( Exception ex ){
                System.out.println( "BBackgroundWorker#WorkerRunner#run(void); ex=" + ex );
            }
        }
    }

    public BBackgroundWorker(){
    }

    public void runWorkAsync(){
        runWorkAsync( null );
    }

    public void runWorkAsync( Object argument ){
        m_runner = new WorkerRunner( doWorkEvent, argument );
        new Thread( m_runner ).start();
    }

    public void reportProgress( int percentProgress ){
        reportProgress( percentProgress, null );
    }
    
    public void reportProgress( int percentProgress, Object userState ){
        BProgressChangedEventArgs e = new BProgressChangedEventArgs( percentProgress, userState );
        try{
            progressChangedEvent.raise( e );
        }catch( Exception ex ){
            System.out.println( "BBackgroundWorker#reportProgress(int,Object); ex=" + ex );
        }
    }
}
