---


---

<h1 id="firmware">Firmware</h1>
<p>Dopo aver configurato la rete ed aver verificato che ogni sua componente sia in grado di comunicare correttamente, il passo successivo è quello di preparare un firmware adatto al nostro sensore.</p>
<p>Al momento in cui questa documentazione viene redatta, la versione di riferimento è la Alpha 1.0; sostanziali modifiche ad essa verranno documentate all’interno del codice, o attraverso documentazione aggiuntiva.</p>
<h2 id="obiettivo">Obiettivo</h2>
<p>Il software che vogliamo andare a realizzare è abbastanza semplice (soprattutto grazie all’aiuto offerto dal framework MySensors).<br>
L’obiettivo del sensore è quello di inviare un messaggio di allarme al gateway solo se viene rilevato un allagamento, altrimenti deve semplicemente inviare un messaggio di keep alive (o heartbeat come viene definito dalla documentazione MySensors), informando il controller che il sensore è correttamente operativo, oltre che comunicare al gateway il livello di batteria (il sensore sarà ovviamente alimentato a batteria), per poi tornare in sleep per un lungo periodo di tempo (a meno di allarmi).</p>
<p>In breve, vogliamo che il sensore rimanga in sleep per lunghi periodi di tempo, e che mandi un keep alive regolarmente, ed un messaggio di allarme nell’eventualità in cui venga rilevato un allagamento (il quale interrompe lo sleep, dunque deve essere configurato tramite interrupt).</p>
<h2 id="codice">Codice</h2>
<p>Si invita a cercare all’interno della repository una versione più aggiornata del codice (se disponibile). Ad ogni modo, si riporta di seguito il codice di riferimento, del quale si procederà a commentarne di seguito le parti più importanti.</p>
<pre><code>#define MY_RADIO_RF24
#define CHILD_WATER 1
#define SLEEP_TIME 10000 //Dummy value 
#define SLEEP_TIME_WATER 1000 //Dummy value
#define INT_PIN 3  //PIN 2 is already busy with RF interrupt

#define MY_DEBUG

#include &lt;MySensors.h&gt;

int8_t wakeupReason = 0;
uint32_t sleepTime = SLEEP_TIME;

void presentation(){
  sendSketchInfo("WaterLeakSensor", "1.0");
  present(CHILD_WATER, S_WATER_LEAK);
}

MyMessage messageWater(CHILD_WATER, V_TRIPPED);


void setup() {
  // put your setup code here, to run once:
  pinMode(3, INPUT_PULLUP);
  send(messageWater.set(0));
}

void loop() {
  // put your main code here, to run repeatedly:
  if (wakeupReason == digitalPinToInterrupt(INT_PIN)){
    //Water!

    /*brief
     * It woke because of interrupt. From now on, it'll wake every SLEEP_TIME_WATER 
     * to check if water is still present.
     * When everything's set, it will resume his sleep. 
     */
     
    sleepTime = SLEEP_TIME_WATER;

    //Now send message to controller
    send(messageWater.set(1));
  } else if (wakeupReason == MY_WAKE_UP_BY_TIMER){
    //Everything's fine 
    sleepTime = SLEEP_TIME;
    send(messageWater.set(0));

    //Report batteryLevel

  }

  //set device to sleep again
  wakeupReason = sleep(digitalPinToInterrupt(INT_PIN), LOW, sleepTime);

}
</code></pre>
<p>Come nell’esempio precedente, procederemo alla dichiarazione del protocollo RF24 e dell’ID del sensore come statico (puramente per fini di debug, questo non influirà in alcun modo sul comportamento del sensore).</p>
<p>Trattandosi di una versione alpha, i valori dei timer sono indicativi, esclusivamente per testarne il corretto funzionamento; in fase di release, tali valori verranno cambiati in maniera coerente al tipo di applicazione.</p>
<p>Questa volta il sensore sarà di tipo S_WATER_LEAK, mentre il tipo di dato che andrà a trasferire sarà di tipo V_TRIPPED, ovvero la variabile può essere “innescata” e “disinnescata”. Il sensore dovrà essere collegato come riportato:</p>

<table>
<thead>
<tr>
<th>Sensor</th>
<th>Arduino</th>
</tr>
</thead>
<tbody>
<tr>
<td><strong>Vdd</strong></td>
<td><strong>5V</strong></td>
</tr>
<tr>
<td><strong>Gnd</strong></td>
<td><strong>Gnd</strong></td>
</tr>
<tr>
<td><strong>S</strong></td>
<td><strong>Pin 3</strong></td>
</tr>
</tbody>
</table><p>ed il pin 3 dovrà essere configurato come input_pullup: questo permetterà di ottenere un livello logico basso quando il sensore rileva dell’acqua (chiudendo il circuito) e alto altrimenti.</p>
<p>In seguito alla fase di setup, il dispositivo andrà in sleep (per una durata SLEEP_TIME). Al risveglio, controllerà la ragione per la quale lo sleep è terminato (timer scaduto o interrupt), decidendo quindi, la tipologia di messaggio da inviare.<br>
Nel caso in cui lo sleep sia terminato a causa di un interrupt collegata al sensore di livello dell’acqua, la durata degli intervalli diventerà più breve, per meglio consentire la rilevazione del momento in cui l’allagamento terminerà (si pensa ad esempio alla creazione di un registro riportante con precisione le fasce orarie di allagamento e di intervento nella risoluzione dello stesso).</p>
<p>La tipologia dei messaggi tra nodo e gateway in questo caso è estremamente semplice: trattandosi infatti di un sensore binario, il contenuto dei messaggi sarà semplicemente (1) se viene rilevata la presenza di acqua, (0) altrimenti.<br>
Nello sketch si fa inoltre riferimento all’invio delle informazioni riguardanti il livello di batteria rimanente: tale funzione è tuttavia ancora in fase di sviluppo (in riferimento al momento in cui questa guida viene redatta), pertanto questo sarà oggetto di una documentazione apposita, rilasciata successivamente.</p>
<p>Trasferendo lo sketch al nostro nodo, con il medesimo setup descritto nella guida precedente, e simulando un interrupt, potremo visualizzare da MyController che il nostro script funziona come previsto.</p>
<p><img src="https://i.imgur.com/m1YkFcw.png" alt="Grafico sensore"></p>

